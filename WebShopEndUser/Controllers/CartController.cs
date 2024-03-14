using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopCore.Helper;
using WebShopCore.Interfaces;
using WebShopCore.Const;
using WebShopCore.ViewModel.Cart;
using WebShopEndUser.Permission;
using WebShopCore.Model;
using WebShopCore.ViewModel.User;
using WebShopCore.Services.VnPayService;

namespace WebShopEndUser.Controllers
{
    [LoginRequired]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IVnPayService _vnPay;
        public CartController(IUnitOfWork uow, IMapper mapper, IVnPayService vnPay)
        {
            _uow = uow;
            _mapper = mapper;
            _vnPay = vnPay;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                CartViewModel model = new();
                var user = HttpContext.Session.GetCurrentAuthentication();
                var userData = _uow.UserRepository.FirstOrDefault(x => x.UserId == user.UserId);
                var cart = _uow.CartRepository
                    .BuildQuery(x => x.CartId == userData.CartId)
                        .Include(x => x.Coupon)
                        .Include(x => x.CartItems)
                            .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.ProductImages)
                            .ThenInclude(x => x.Image)
                    .FirstOrDefault();
                if(cart != null)
                {
                    await UpdateCartTotalPrice(cart.CartId);
                }
                model = _mapper.Map<CartViewModel>(cart);
                return View(model);
            }
            catch (Exception ex)
            {
                return Redirect("/Auth/Login");
            }
        }

        public async Task<IActionResult> CreateCart(int productId)
        {
            var user = HttpContext.Session.GetCurrentAuthentication();
            if (productId != 0)
            {
                await AddToCart(user.UserId, productId);

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            var cartItem = _uow.CartItemRepository.FirstOrDefault(x => x.CartItemId == cartItemId);
            if(cartItem != null)
            {
                var product = _uow.ProductRepository.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
                if(product != null)
                {
                    product.Quantity++;
                    await _uow.CommitAsync();
                }
                var cart = _uow.CartRepository.BuildQuery(x => x.CartId == cartItem.CartId)
                    .Include(x => x.CartItems)
                    .FirstOrDefault();

                if(cart != null)
                {
                    if(cart.CartItems.Count >= 2) 
                    {
                        cart.Total -= product.SellPrice;
                        _uow.CartItemRepository.DeleteById(cartItem.CartItemId);
                        await _uow.CommitAsync();
                    }
                    else
                    {
                        var user = HttpContext.Session.GetCurrentAuthentication();
                        if(user != null)
                        {
                            var userData = _uow.UserRepository.FirstOrDefault(x => x.UserId == user.UserId);
                            userData.CartId = null;
                        }
                        _uow.CartItemRepository.DeleteById(cartItem.CartItemId);
                        _uow.CartRepository.DeleteById(cart.CartId);
                        await _uow.CommitAsync();
                    }
                }
                await _uow.CommitAsync();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateCartItem(int cartItemId, bool isAdd)
        {
            var cartItem = _uow.CartItemRepository.FirstOrDefault(x => x.CartItemId == cartItemId);
            if(cartItem != null)
            {
                var product = _uow.ProductRepository.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
                var cart = _uow.CartRepository.BuildQuery(x => x.CartId == cartItem.CartId)
                    .Include(x => x.CartItems)
                    .FirstOrDefault();
                if(product != null)
                {
                    if (isAdd)
                    {
                        if(product.Quantity > 0)
                        {
                            product.Quantity--;
                            cartItem.Quantity++;
                        }
                        else
                        {
                            throw new Exception("Sản phẩm không đủ số lượng");
                        }
                    }
                    else
                    {
                        if(cart.CartItems.Count >= 2)
                        {
                            if(cartItem.Quantity > 1)
                            {
                                cartItem.Quantity--;
                                product.Quantity++;
                            }
                            else
                            {
                                product.Quantity++;
                                _uow.CartItemRepository.DeleteById(cartItem.CartItemId);
                            }
                        }
                        else
                        {
                            product.Quantity++;
                            var user = HttpContext.Session.GetCurrentAuthentication();
                            if (user != null)
                            {
                                var userData = _uow.UserRepository.FirstOrDefault(x => x.UserId == user.UserId);
                                userData.CartId = null;
                            }
                            _uow.CartItemRepository.DeleteById(cartItem.CartItemId);
                            _uow.CartRepository.DeleteById(cart.CartId);
                        }
                    }

                }
                await _uow.CommitAsync();
            }

            return RedirectToAction("Index");
        }
        private async Task<bool> AddToCart(int userId, int productId)
        {
            var user = _uow.UserRepository.FirstOrDefault(x => x.UserId == userId);
            if (user != null)
            {
                var product = _uow.ProductRepository
                    .FirstOrDefault(x => x.ProductId == productId 
                    && x.IsActive && !x.IsDeleted && x.Quantity > 0);
                if(product == null)
                {
                    throw new Exception("Sản phẩm không đủ số lượng");
                }
                var checkCart = _uow.CartRepository.BuildQuery(x => x.CartId == user.CartId)
                    .Include(x => x.CartItems)
                    .FirstOrDefault();
                if (checkCart == null)
                {
                    Cart cart = new()
                    {
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        CouponId = null
                    };
                    _uow.CartRepository .Add(cart);
                    await _uow.CommitAsync();

                    CartItem cartItem = new()
                    {
                        ProductId = product.ProductId,
                        Quantity = 1,
                        Cart = cart,
                        CartId = cart.CartId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false
                    };
                    _uow.CartItemRepository .Add(cartItem);
                    await _uow.CommitAsync();

                    cart.Total = product.SellPrice;
                    user.CartId = cart.CartId;
                    await _uow.CommitAsync();
                }
                else
                {
                    if(checkCart.CartItems.Any(x => x.ProductId == productId))
                    {
                        foreach(var item in checkCart.CartItems)
                        {
                            if(item.ProductId == productId)
                            {
                                item.Quantity++;
                            }
                        }
                    }
                    else
                    {
                        CartItem newCartItem = new()
                        {
                            ProductId = product.ProductId,
                            Quantity = 1,
                            CartId = checkCart.CartId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            IsActive = true,
                            IsDeleted = false
                        };
                        _uow.CartItemRepository.Add(newCartItem);
                    }
                }
                product.Quantity--;
                await _uow.CommitAsync();
            }
            return true;
        }
        private async Task<bool> UpdateCartTotalPrice(int cartId)
        {
            var cart = _uow.CartRepository
                .BuildQuery(x => x.CartId == cartId)
                .Include(x => x.Coupon)
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.Product)
                .FirstOrDefault();
            if(cart != null)
            {
                cart.Total = 0;
                if(cart.CartItems != null && cart.CartItems.Any())
                {
                    foreach(var cartItem in cart.CartItems)
                    {
                        cart.Total += cartItem.Quantity * cartItem.Product.SellPrice;
                    } 

                    var coupon = _uow.CouponRepository.FirstOrDefault(x => x.Id == cart.CouponId);
                    if(coupon != null)
                    {
                        if(coupon.CouponPriceType == (int)SysEnum.CouponType.Direct)
                        {
                            if(coupon.CouponPriceType >= cart.Total)
                            {
                                cart.Total = 0;
                            }
                            else
                            {
                                cart.Total -= coupon.CouponPriceType;
                            }
                        }
                        else if(coupon.CouponPriceType == (int)SysEnum.CouponType.Percent)
                        {
                            cart.Total -= cart.Total * coupon.CouponPriceType / 100;
                        }
                    }
                }
            }
            await _uow.CommitAsync();
            return true;
        }

        public IActionResult Checkout()
        {
            var user = HttpContext.Session.GetCurrentAuthentication();
            UserViewModel model = new();
            if (user != null)
            {
                var userData = _uow.UserRepository
                    .BuildQuery(x => x.UserId == user.UserId)
                    .Include(x => x.Cart)
                        .ThenInclude(x => x.CartItems)
                        .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductImages)
                        .ThenInclude(x => x.Image)
                    .FirstOrDefault();
                model = _mapper.Map<UserViewModel>(userData);

            }
            return View(model);
        }
        public async Task<IActionResult> SettleUp(int payment)
        {
            var user = HttpContext.Session.GetCurrentAuthentication();
            UserViewModel userModel = new();
            var tmpCartId = 0;
            var userData = _uow.UserRepository
                    .BuildQuery(x => x.UserId == user.UserId)
                    .Include(x => x.Cart)
                        .ThenInclude(x => x.CartItems)
                        .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductImages)
                        .ThenInclude(x => x.Image)
                    .FirstOrDefault();
            userModel = _mapper.Map<UserViewModel>(userData);
            tmpCartId = userData.CartId.Value;
            if (user == null)
            {
                return NotFound("Lỗi không tìm thấy người dùng");
            }
            bool result;
            if (payment == (int)SysEnum.PaymentStatus.Pending)
            {
                result = await CreateOrder(userData, payment, null) ;
                if(result == false)
                {
                    return NotFound("Lỗi không tạo được đơn hàng...");
                }
                await _uow.CommitAsync();
            }
            if (payment == (int)SysEnum.PaymentStatus.Completed)
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = (double)userModel.Cart.Total,
                    CreatedDate = DateTime.Now,
                    Description = $"{user.Name} {user.Phone}",
                    FullName = user.Name,
                    OrderId = new Random().Next(1000, 100000)
                };
                return Redirect(_vnPay.CreatePaymenUrl(HttpContext, vnPayModel));
            }
            

            return RedirectToAction("OrderHistory", "Home");
        }
        public async Task<IActionResult> PaymentCallBack()
        {
            var user = HttpContext.Session.GetCurrentAuthentication();
            var userData = _uow.UserRepository.FirstOrDefault(x => x.UserId == user.UserId);
            if(userData == null)
            {
                return NotFound("Lỗi không tìm thấy người dùng");
            }
            var response = _vnPay.PaymentExecute(Request.Query);
            if(response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VnPay";
                return View();
            }

            else
            {
                var result = CreateOrder(userData, 1, Int64.Parse(response.OrderId)).Result;
                if (result == false)
                {
                    return NotFound("Lỗi không tạo được đơn hàng. Vui lòng liên hệ với admin...");
                }
                await _uow.CommitAsync();
                //TempData["Message"] = $"Thanh toán thành công: {response.VnPayResponseCode}";
            }
            return RedirectToAction("OrderHistory", "Home");
        }

        private async Task<bool> CreateOrder(User user, int statusOrder, long? orderId)
        {
            try
            {
                var userData = _uow.UserRepository.BuildQuery(x => x.UserId == user.UserId)
                    .Include(x => x.Cart)
                        .ThenInclude(x => x.CartItems)
                        .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductImages)
                        .ThenInclude(x => x.Image)
                    .FirstOrDefault();
                var tmpId = new Random().Next(1000, 100000);
                Order order = new()
                {
                    OrderId = orderId.HasValue ? orderId.Value : tmpId,
                    Name = "",
                    Code = "OD" + DateTime.Now.ToString("yyyyMMddHHmmss") + userData.UserId,
                    ShippingStatus = (int)SysEnum.ShippingStatus.WaitingForConfirm,
                    CreateByUserId = userData.UserId,
                    OrderStatus = (int)SysEnum.OrderStatus.Pending,
                    Address = userData.Address
                };
                if (statusOrder == (int)SysEnum.PaymentStatus.Pending)
                {
                    order.PaymentStatus = (int)SysEnum.PaymentStatus.Pending;
                    _uow.OrderRepository.Add(order);
                }
                else if (statusOrder == (int)SysEnum.PaymentStatus.Completed)
                {
                    order.PaymentStatus = (int)SysEnum.PaymentStatus.Completed;
                    _uow.OrderRepository.Add(order);
                }

                List<int> productIdsData = new();
                foreach (var item in userData.Cart.CartItems)
                {
                    productIdsData.Add(item.ProductId.Value);
                }
                foreach (var item in productIdsData)
                {
                    var product = _uow.ProductRepository.GetById(item);
                    if (product != null)
                    {
                        var orderItem = new OrderItem()
                        {
                            ProductId = product.ProductId,
                            Price = product.SellPrice,
                            PriceTotal = product.SellPrice,
                            Quantity = 1,
                            Order = order,
                            OrderId = order.OrderId
                        };
                        _uow.OrderItemRepository.Add(orderItem);
                    }
                }
                order.Total = userData.Cart.Total;
                var cartItem = _uow.CartItemRepository.BuildQuery(x => x.CartId == userData.CartId).ToList();
                if (cartItem != null && cartItem.Any())
                {
                    foreach (var item in cartItem)
                    {
                        _uow.CartItemRepository.Delete(item);
                    }
                }
                userData.CartId = null;
                _uow.CartRepository.Delete(userData.Cart);
                await _uow.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
