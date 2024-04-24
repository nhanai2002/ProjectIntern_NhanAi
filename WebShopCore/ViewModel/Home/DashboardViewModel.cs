using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.ViewModel.Order;
using WebShopCore.ViewModel.Product;
using WebShopCore.ViewModel.User;

namespace WebShopCore.ViewModel.Home
{
    public class DashboardViewModel
    {
        public int products { get; set; }
        public string Revenue { get; set; }
        public int users { get; set; }
        public int orders { get; set; }
        public decimal[] totals { get; set; }
        public int[] TotalSold { get; set; }
        public List<UserDashboardViewModel>? ListUsers { get; set; }
        public List<ProductDashboardViewModel>? ListProducts { get; set; }
    }

    public class ProductDashboardViewModel
    {
        public int? ProductId { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
        public int TotalSold { get; set; }
        public decimal ProductRevenue { get; set; }
        public DateTime? OrderDate { get; set; }
    }

    public class UserDashboardViewModel
    {
        public int? UserId { get; set; }
        public string Name { get; set; }
        public int Purchases { get; set; }
        public decimal Total { get; set; }
    }

}
