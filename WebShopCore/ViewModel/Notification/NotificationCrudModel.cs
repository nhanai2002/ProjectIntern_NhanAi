using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.ViewModel.User;

namespace WebShopCore.ViewModel.Notification
{
    public class NotificationCrudModel : BaseNotification
    {
        public int? UserId { get; set; }
        public List<UserViewModel>? Users { get; set; }
        public List<UserViewModel>? ListUserVM { get; set; }
    }
}
