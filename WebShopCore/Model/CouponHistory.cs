using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseCouponHistory
    {
        [Key]
        public int Id { get; set; }
        public int? CouponId { get; set; }
        public int? UserId { get; set; }
    }
    public class CouponHistory : BaseCouponHistory
    {
        [ForeignKey(nameof(CouponId))]
        public Coupon Coupon { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

    }
}
