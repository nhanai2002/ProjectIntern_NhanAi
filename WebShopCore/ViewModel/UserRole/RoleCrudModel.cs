using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;

namespace WebShopCore.ViewModel.UserRole
{
    public class RoleCrudModel : BaseRole
    {
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string[]? SelectedActions { get; set; }
        public List<ControllerInfo>? ctrls {  get; set; }
        public List<ActionInfo>? actions {  get; set; }
        public List<RoleViewModel>? roles {  get; set; }
    }
    public class ControllerInfo
    {
        public string ControllerName { get;set; }
        public string DisplayName { get;set; }
        public List<ActionInfo> Actions { get; set; }
    }
    public class ActionInfo
    {
        public string ActionName { get;set; }
        public string DisplayName { get;set; }
        public bool isChecked { get; set; }

    }
}
