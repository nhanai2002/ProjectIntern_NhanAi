using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseUserNoti : BaseClass
    {
        [Key]
        public int IdUserNoti { get; set; }
        public int? UserId { get; set; }
        public int? NotiId { get; set; }

    }

    public class UserNoti : BaseUserNoti
    {
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [ForeignKey(nameof(NotiId))]
        public Notification Notification { get; set; }
    }
}
