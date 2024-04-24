using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;
using WebShop.Models;
using WebShop.Permission;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.ViewModel.Home;

namespace WebShop.Controllers
{
    [LoginRequired]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;        

        public HomeController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel();

            int products = _uow.ProductRepository.BuildQuery(x => !x.IsDeleted).Count();
            string revenue = _uow.OrderRepository.GetAll().DefaultIfEmpty().Sum(x => x?.Total ?? 0).FormatCurrency();
            int orders = _uow.OrderRepository.GetAll().Count();
            int users = _uow.UserRepository.BuildQuery(x => x.RoleId == (int)SysEnum.DefaultRole.EndUser).Count();

            var topProducts = _uow.OrderItemRepository.BuildQuery(x => !x.IsDeleted)
                .Include(x => x.Product)
                .GroupBy(x => x.ProductId)
                .Select(x => new ProductDashboardViewModel
                { 
                    ProductId = x.Key,
                    Name = x.First().Product.Name,
                    TotalSold = x.Sum(p => p.Quantity),
                    ProductRevenue = x.Sum(p=> p.PriceTotal)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(7)
                .ToList();
            
            var topUsers = _uow.OrderRepository.BuildQuery(x => !x.IsDeleted)
                .Include(x => x.User)
                .GroupBy(x => x.CreateByUserId)
                .Select(x => new UserDashboardViewModel
                {
                    UserId = x.Key,
                    Name = x.First().User.Name,
                    Purchases = x.Count(),
                    Total = x.Sum(p => p.Total)
                })
                .Take(50)
                .ToList();

            var totals = _uow.OrderRepository.BuildQuery(x => x.CreatedAt.Year == DateTime.Now.Year)
                .GroupBy(x => x.CreatedAt.Month)
                .OrderBy(x => x.Key)
                .Select(x => new { Month = x.Key, Total = x.Sum(p => p.Total)})
                .ToArray();

            var totalsresult = new decimal[12];
            foreach(var i in totals)
            {
                totalsresult[i.Month - 1] = i.Total;
            }

            var totalSold = _uow.OrderItemRepository.BuildQuery(x => x.Order.CreatedAt.Year == DateTime.Now.Year)
                .GroupBy(x => x.Order.CreatedAt.Month)
                .OrderBy(x => x.Key)
                .Select(x => new { Month = x.Key, Total = x.Sum(p => p.Quantity) })
                .ToArray();
            var totalSoldResult = new int[12];
            foreach(var i in totalSold)
            {
                totalSoldResult[i.Month-1] = i.Total;
            }

            model.products = products;
            model.users = users;
            model.Revenue = revenue;
            model.orders = orders;
            model.totals = totalsresult.Select(x => Math.Round(x, 2)).ToArray();
            model.TotalSold = totalSoldResult;
            model.ListProducts = topProducts;
            model.ListUsers = topUsers;


            return View(model);
        }
        public IActionResult ProhibitAccess()
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