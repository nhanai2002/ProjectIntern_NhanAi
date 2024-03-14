using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseCart : BaseClass
    {
        [Key]
        public int CartId { get; set; }
        public decimal Total { get; set; }
        public int? CouponId { get; set; }
    }

    public class Cart : BaseCart
    {
        [ForeignKey(nameof(CouponId))]
        public Coupon Coupon { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}
