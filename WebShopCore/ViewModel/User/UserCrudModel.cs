using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;

namespace WebShopCore.ViewModel.User
{
    public class UserCrudModel : BaseUser
    {
        public IFormFile? FileImage { get; set; }
        public string? ErrorMessage { get; set; }

    }
}
