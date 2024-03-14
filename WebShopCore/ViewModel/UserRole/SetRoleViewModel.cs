using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;

namespace WebShopCore.ViewModel.UserRole
{
    public class SetRoleViewModel : BaseRole
    {
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public List<RoleViewModel>? Roles { get; set; }
    }
}
