using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.ViewModel.Product;

namespace WebShopCore.ViewModel.Order
{
    public class OrderItemViewModel : BaseOrderItem
    {
        public OrderViewModel Order { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
