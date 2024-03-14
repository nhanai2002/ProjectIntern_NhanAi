using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Helper
{
    public static class Extensions
    {
        public static string GetEnumDisplayName(this Enum enumType)
        {
            return enumType.GetType().GetMember(enumType.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>().Name;
        }
        public static string HasPassword(this string password)
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
        public static string ConvertToVND(this decimal price)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            if(price >= 0)
            {
                return String.Format(info, "{0:c}", price);
            }
            return "Check your input price";
        }
    }
}
