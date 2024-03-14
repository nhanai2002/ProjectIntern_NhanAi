using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.ViewModel.Auth
{
    public class ChangePasswordViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set;}
        public string? Email { get; set; }
        public string Password { get; set;}
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp")]
        public string ConfirmPassword { get; set; }

    }
}
