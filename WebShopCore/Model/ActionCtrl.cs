using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseActionCtrl : BaseClass
    {
        [Key]
        public int ActionCtrlId { get; set; }
        public string ActionName { get; set; }
    }

    public class ActionCtrl : BaseActionCtrl
    {
        public ICollection<ActionRole> ActionRole { get; set; }

    }

}
