using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseCartHistory
    {
        [Key]
        public int IdCartHistory { get; set; }
        public string CartHistoryJson { get; set; }
        public DateTime CheckoutAt { get; set; }

    }

    public class CartHistory : BaseCartHistory
    {

    }
}
