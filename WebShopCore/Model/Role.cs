using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseRole : BaseClass
    {
        [Key]
        public int RoleId { get;set; }
        public string Name { get; set; }
    }

    public class Role : BaseRole
    {
        public ICollection<ActionRole> ActionRole { get; set; }
    }
}
