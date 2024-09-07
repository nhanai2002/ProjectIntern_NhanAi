using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.ViewModel.Notification;

namespace WebShopCore.Services.Hubs
{
    public interface INotificationHub
    {
        Task SendNotificationToAll(NotificationViewModel data);
        Task SendNotificationToClient(NotificationViewModel data, int userId);
    }
}
