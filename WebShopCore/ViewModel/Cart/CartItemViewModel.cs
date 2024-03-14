using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.ViewModel.Product;

namespace WebShopCore.ViewModel.Cart
{
    public class CartItemViewModel : CartItem
    {
        public ProductViewModel product { get; set; }
    }
}
