using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseHubConnection : BaseClass
    {
        [Key]
        public int Id { get; set; }
        public string ConnectionId { get; set; }
        public int UserId { get; set; }
    }
    public class HubConnection : BaseHubConnection
    {
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
