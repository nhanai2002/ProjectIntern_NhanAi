using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Model;
using WebShopCore.ViewModel.Cart;

namespace WebShopCore.ViewModel.User
{
    public class UserViewModel : BaseUser
    {
        public string BirthDayDisplay
        {
            get
            {
                return BirthDay.ToString("dd/MM/yyyy");
            }
        }
        public CartViewModel Cart { get; set; }
        public string EnumGenderDisplay => ((SysEnum.Gender)Enum.Parse(typeof(SysEnum.Gender), this.Gender.ToString())).GetEnumDisplayName();
        public string StatusDisplay
        {
            get
            {
                if (IsActive)
                {
                    return "Đang hoạt động";
                }
                else
                {
                    return "Bị khóa";
                }
            }
        }


    }
}
