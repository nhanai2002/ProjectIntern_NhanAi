using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebShop.Models;
using WebShop.Permission;
using WebShopCore.Const;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.Repositories;
using WebShopCore.ViewModel.Order;
using WebShopCore.ViewModel.Product;
using WebShopCore.ViewModel.User;

namespace WebShop.Controllers
{
    [LoginRequired]
    [Display(Name = "Đơn hàng")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public OrderController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        [Display(Name = "Xem danh sách đơn hàng")]

        public IActionResult Index(string keyword, int? paymentStatus, int? orderStatus, int? shippingStatus)
        {
            var data = _uow.OrderRepository.BuildQuery(x => !x.IsDeleted);
            if(keyword != null && keyword != "")
            {
                data = data.Where(x => EF.Functions.Like(x.Code!, $"{keyword}"));
            }
            if(paymentStatus != null)
            {
                data = data.Where(x => x.PaymentStatus == paymentStatus);
            }
            if(orderStatus != null)
            {
                data = data.Where(x => x.OrderStatus == orderStatus);
            }
            if (shippingStatus != null)
            {
                data = data.Where(x => x.ShippingStatus == shippingStatus);
            }
            data = data.Include(x => x.OrderItems).ThenInclude(x => x.Product);
            data = data.OrderByDescending(x => x.CreatedAt);
            var rs = _mapper.Map<List<OrderViewModel>>(data.ToList());
            return View(rs);
        }


        [Display(Name = "Tạo đơn hàng")]
        public IActionResult Create()
        {
            var model = new OrderCrudModel();
            model.ListUserViewModel = _uow.UserRepository.BuildQuery(x => !x.IsDeleted 
            && x.RoleId == (int)SysEnum.DefaultRole.EndUser
            && x.IsActive)
                .Select(x => _mapper.Map<UserViewModel>(x))
                .ToList();
            model.ListProductViewModel = _uow.ProductRepository.BuildQuery(x => !x.IsActive && x.Quantity > 0)
                .Select(x => _mapper.Map<ProductViewModel>(x))
                .ToList();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(OrderCrudModel model)
        {
            var error = new ErrorViewModel();
            try
            {
                Order order = new()
                {
                    // sửa ở đây
                    OrderId = new Random().Next(1000, 100000),
                    Name = "",
                    Code = "OD" + DateTime.Now.ToString("yyyyMMddHHmmss") + model.CreateByUserId,
                    PaymentStatus = model.PaymentStatus,
                    ShippingStatus = model.ShippingStatus,
                    CreateByUserId = model.CreateByUserId,
                    OrderStatus = model.OrderStatus,
                    Address = model.Address
                };
                _uow.OrderRepository.Add(order);
                await _uow.CommitAsync();
                foreach (var id in model.ProductIds)
                {
                    var convertToInt = Int32.Parse(id);
                    var product = _uow.ProductRepository.GetById(convertToInt);
                    if (product != null)
                    {
                        OrderItem orderItem = new()
                        {
                            ProductId = product.ProductId,
                            Price = product.SellPrice,
                            PriceTotal = product.SellPrice,
                            Quantity = 1,
                            Order = order,
                            OrderId = order.OrderId
                        };
                        _uow.OrderItemRepository.Add(orderItem);
                        orderItem.PriceTotal += product.SellPrice;
                    }
                }
                await _uow.CommitAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                error.ErrorMessage = ex.Message;
                return View("Error", error);
            }
        }


        [Display(Name = "Xóa đơn hàng")]
        public async Task<IActionResult> Delete(long orderId)
        {
            var order = _uow.OrderRepository.FirstOrDefault(x => x.OrderId == orderId);
            if (order != null)
            {
                order.IsDeleted = true;
            }
            await _uow.CommitAsync();
            return RedirectToAction("Index");
        }

        [Display(Name = "Sửa đơn hàng")]
        public IActionResult Update(long orderId)
        {
            var model = new OrderCrudModel();
            var order = _uow.OrderRepository.BuildQuery(x => x.OrderId == orderId)
                .Include(x => x.OrderItems)
                   .ThenInclude(x => x.Product)
               .FirstOrDefault();
            if(order != null)
            {
                model = _mapper.Map<OrderCrudModel>(order);
            }
            model.ListUserViewModel = _uow.UserRepository
                .BuildQuery(x => !x.IsDeleted && x.RoleId != (int)SysEnum.DefaultRole.Admin && x.IsActive)
                .Select(x => _mapper.Map<UserViewModel>(x))
                .ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(OrderViewModel model)
        {
            var order = _uow.OrderRepository.FirstOrDefault(x => x.OrderId == model.OrderId);   
            if (order != null)
            {
                order.Address = model.Address;
                order.IsActive = true;
                order.PaymentStatus = model.PaymentStatus;
                order.ShippingStatus = model.ShippingStatus;
                order.OrderStatus = model.OrderStatus;
                order.CreateByUserId = model.CreateByUserId;
            }
            await _uow.CommitAsync();
            return RedirectToAction("Index");
        }
    }
}
