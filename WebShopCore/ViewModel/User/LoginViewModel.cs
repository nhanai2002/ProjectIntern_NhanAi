using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.ViewModel.User
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
