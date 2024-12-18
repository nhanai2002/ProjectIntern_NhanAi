﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Interfaces;
using WebShopCore.ViewModel.Notification;

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

        public static string FormatCurrency(this decimal amount)
        {
            string suffix = "";

            if (amount >= 1000000000)
            {
                suffix = " B";
                amount /= 1000000000;
            }
            else if (amount >= 1000000)
            {
                suffix = " M";
                amount /= 1000000;
            }

            return string.Format("{0:N0}{1}", amount, suffix);
        }

    }
}
