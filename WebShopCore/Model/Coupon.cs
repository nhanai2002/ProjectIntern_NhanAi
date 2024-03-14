using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseCoupon : BaseClass
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(255, ErrorMessage = "Không được vượt quá 255 ký tự")]
        public string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string Code { get; set; }
        [Required]
        public int Status { get; set; }
        public int LimitationTimes { get; set; }
        public int CouponPriceValue { get; set; }
        public int CouponPriceType { get; set; }
        public int DiscountLimitationType { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
    }
    public class Coupon : BaseCoupon
    {
    }
}
