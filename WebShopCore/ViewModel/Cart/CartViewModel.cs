using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.ViewModel.Coupon;

namespace WebShopCore.ViewModel.Cart
{
    public class CartViewModel : BaseCart
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public CouponViewModel coupon { get; set; }
    }
}
