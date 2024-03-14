using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseCartItem : BaseClass
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
        public int? CartId { get; set; }
        public int? ProductId { get; set; }

    }
    public class CartItem : BaseCartItem
    {
        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
