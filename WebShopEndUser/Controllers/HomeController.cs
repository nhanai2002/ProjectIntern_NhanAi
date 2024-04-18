using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using WebShopCore.Helper;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.ViewModel.Category;
using WebShopCore.ViewModel.Feedback;
using WebShopCore.ViewModel.Order;
using WebShopCore.ViewModel.Product;
using WebShopEndUser.Models;
using WebShopEndUser.Permission;

namespace WebShopEndUser.Controllers
{
    //[LoginRequired]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public HomeController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public IActionResult Index(string keyword, int[]? SelectedKeyword)
        {
            var data = _uow.ProductRepository
                .BuildQuery(x => x.IsActive && !x.IsDeleted);
            var categories = _uow.CategoryRepository
                .BuildQuery(x => x.IsActive && !x.IsDeleted)
                .Select(x => _mapper.Map<CategoryViewModel>(x))
                .ToList();

            ViewData["CategoryList"] = categories;
            
            if(keyword != null && keyword != "")
            {
                data = data.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%"));
            }
            data = data.Include(x => x.ProductCategories)
                            .ThenInclude(x => x.Category)
                       .Include(x => x.ProductImages)
                            .ThenInclude(x => x.Image);
            if (SelectedKeyword.Any(x => x != null))
            {
                var rs = _uow.CategoryRepository.BuildQuery(x => SelectedKeyword.Contains(x.CategoryId)).Select(x => x.CategoryId).ToList();
                if(rs != null)
                {
                    var getData = data.Where(x => x.ProductCategories.Any(p => rs.Contains(p.Category.CategoryId)));
                    if(getData != null)
                    {
                        data = getData;
                    }
                }
            }
            data = data.OrderByDescending(x => x.CreatedAt);
            var result = _mapper.Map<List<ProductViewModel>>(data.ToList());
            return View(result);
        }


        public IActionResult Details(int productId)
        {
            var product = _uow.ProductRepository
                .BuildQuery(x => x.ProductId == productId && x.IsActive && !x.IsDeleted)
                .Include(x => x.ProductImages)
                    .ThenInclude(x => x.Image)
                .Select(x => _mapper.Map<ProductViewModel>(x))
                .FirstOrDefault();
            if (product == null) return NotFound("Lỗi không tìm thấy chi tiết sản phẩm");

            var productFeedback = _uow.ProductFeedBackRepository
                .BuildQuery(x => x.ProductId == productId)   
                .Select(x => x.FeedbackId)
                .ToList();
            var feedback = _uow.FeedbackRepository.BuildQuery(x => productFeedback.Contains(x.FeedbackId))
                .Include(x => x.User)
                .Include(x => x.Images)
                .Select(x => _mapper.Map<FeedbackViewModel>(x))
                .ToList();
            product.Feedbacks = feedback;

            return View(product);
        }


        [LoginRequired]
        public IActionResult OrderHistory()
        {
            var user = HttpContext.Session.GetCurrentAuthentication();
            var order = _uow.OrderRepository.BuildQuery(x => x.CreateByUserId == user.UserId && !x.IsDeleted)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                    .ThenInclude(x => x.Image)
                .Select(x => _mapper.Map<OrderViewModel>(x))
                .ToList();
            return View(order);
        }
        public IActionResult Privacy()
        {
            return View();
        }
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}