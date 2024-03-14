using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Model;
using WebShopCore.ViewModel.User;

namespace WebShopCore.ViewModel.Order
{
    public class OrderViewModel : BaseOrder
    {
        public UserViewModel User { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public string OrderStatusDisplay => ((SysEnum.OrderStatus)Enum.Parse(typeof(SysEnum.OrderStatus), this.OrderStatus.ToString())).GetEnumDisplayName();
        public string PaymentStatusDisplay => ((SysEnum.PaymentStatus)Enum.Parse(typeof(SysEnum.PaymentStatus), this.PaymentStatus.ToString())).GetEnumDisplayName();
        public string ShippingStatusDisplay => ((SysEnum.ShippingStatus)Enum.Parse(typeof(SysEnum.ShippingStatus), this.ShippingStatus.ToString())).GetEnumDisplayName();

    }
}
