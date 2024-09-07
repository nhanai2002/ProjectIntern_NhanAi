using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.ViewModel.Notification;

namespace WebShopCore.ViewModel.Auth
{
    public class AuthenticationModel
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string AvatarUrl {  get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public DateTime? VaildTime { get; set; }
        public DateTime? SubscriptionEndAt { get; set; }
        public int RoleId { get; set; }
        //public List<NotificationViewModel> Notify { get; set; }
    }
}
