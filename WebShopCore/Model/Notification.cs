using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseNotification : BaseClass
    {
        [Key]
        public int NotiId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int MessageType { get; set; }
        public bool Seen { get; set; } = false;
        public DateTime? SendAt { get; set; }
    }

    public class Notification : BaseNotification
    {
        public ICollection<UserNoti> UserNoti { get; set; }
    }
}
