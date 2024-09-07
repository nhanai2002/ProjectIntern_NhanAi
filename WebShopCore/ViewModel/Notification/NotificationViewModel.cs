using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Const;
using WebShopCore.Helper;
using WebShopCore.Model;

namespace WebShopCore.ViewModel.Notification
{
    public class NotificationViewModel : BaseNotification
    {
        public string NotifyTypeDisplay
        {
            get
            {
                switch(MessageType)
                {
                    case (int)SysEnum.NotificationType.All: 
                        return SysEnum.NotificationType.All.GetEnumDisplayName();
                    case (int)SysEnum.NotificationType.Personal:
                        return SysEnum.NotificationType.Personal.GetEnumDisplayName();
                    default:
                        return SysEnum.NotificationType.Group.GetEnumDisplayName();
                }
            }
        }
    }
}
