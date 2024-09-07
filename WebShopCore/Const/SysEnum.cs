using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Const
{
    public class SysEnum
    {
        public enum Gender
        {
            [Display(Name = "Nam")]
            Male = 0,
            [Display(Name = "Nữ")]
            Female = 1,
            [Display(Name = "Khác")]
            Other = 2,
        }
        public enum DefaultRole
        {
            [Display(Name = "Administrators")]
            Admin = 1,
            [Display(Name = "User")]
            EndUser = 2
        }

        public enum ProductStatus
        {
            [Display(Name = "Tất cả")]
            All = 0,
            [Display(Name = "Đang hoạt động")]
            Active = 1,
            [Display(Name = "Hết hàng")]
            OutOfStock = 2,
            [Display(Name = "Đã ẩn")]
            Hidden = 3
        }

        public enum NotificationType
        {
            [Display(Name = "Tất cả")]
            All = 0,
            [Display(Name = "Cá nhân")]
            Personal = 1,
            [Display(Name = "Nhóm")]
            Group = 2
        }

        public enum CouponType
        {
            [Display(Name = "Giảm thẳng")]
            Direct = 0,
            [Display(Name = "Giảm phần trăm")]
            Percent = 1,
        }
        public enum CouponStatus
        {
            [Display(Name = "Tất cả")]
            All = 0,
            [Display(Name = "Đang diễn ra")]
            Happening = 1,
            [Display(Name = "Kết thúc")]
            End = 2,
        }



        public enum OrderStatus
        {
            [Display(Name = "Đang xử lý")]
            Pending = 0,
            [Display(Name = "Đã hủy")]
            Cancel = 1,
            [Display(Name = "Đã hoàn thành")]
            Completed = 2,

        }
        public enum PaymentStatus
        {
            [Display(Name = "Thanh toán trực tiếp")]
            Pending = 0,
            [Display(Name = "Đã thanh toán")]
            Completed = 1,
        }
        public enum ShippingStatus
        {
            [Display(Name = "Chờ xác nhận")]
            WaitingForConfirm = 0,
            [Display(Name = "Chờ lấy hàng")]
            WaitingForTake = 1,
            [Display(Name = "Đang giao")]
            InProgress = 2,
            [Display(Name = "Đã giao")]
            Completed = 3,
            [Display(Name = "Đã hủy")]
            Cancel = 4,
            [Display(Name = "Hoàn tiền")]
            Refund = 5,

        }

    }
}
