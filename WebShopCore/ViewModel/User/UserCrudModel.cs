using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;

namespace WebShopCore.ViewModel.User
{
    public class UserCrudModel : BaseUser
    {
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp")]
        public string? ConfirmPassword { get; set; }
        public IFormFile? FileImage { get; set; }
        public string? ErrorMessage { get; set; }

    }
}
