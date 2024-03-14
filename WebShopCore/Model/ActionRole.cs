using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseActionRole : BaseClass
    {
        [Key]
        public int ActionRoleId { get; set; }
        public int? RoleId { get; set; } 
        public int? ActionCtrlId { get; set; }

    }
    public class ActionRole : BaseActionRole
    {
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        [ForeignKey(nameof(ActionCtrlId))]
        public ActionCtrl ActionCtrl { get; set; }

    }
}
