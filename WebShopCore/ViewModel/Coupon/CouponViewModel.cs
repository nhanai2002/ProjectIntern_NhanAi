using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Model;

namespace WebShopCore.ViewModel.Coupon
{
    public class CouponViewModel : BaseCoupon
    {
        public string CouponPriceTypeDisplay
        {
            get
            {
                switch (CouponPriceType)
                {
                    case (int)SysEnum.CouponType.Direct:
                        return SysEnum.CouponType.Direct.GetEnumDisplayName();
                    default:
                        return SysEnum.CouponType.Percent.GetEnumDisplayName();
                }
            }

        }
    }
}
