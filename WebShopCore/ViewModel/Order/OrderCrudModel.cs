using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.ViewModel.Product;
using WebShopCore.ViewModel.User;

namespace WebShopCore.ViewModel.Order
{
    public class OrderCrudModel : BaseOrder
    {
        public UserViewModel User { get; set; }
        public List<UserViewModel> ListUserViewModel { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public List<ProductViewModel> ListProductViewModel { get; set; }
        public string ProductIds { get; set; }
    }
}
