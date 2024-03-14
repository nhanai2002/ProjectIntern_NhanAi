using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseOrder : BaseClass
    {
        [Key]
        public long OrderId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string Code { get; set; }
        [Required]
        public int OrderStatus { get; set; }
        [Required]
        public int ShippingStatus { get; set; }
        [Required]
        public int PaymentStatus { get; set; }
        public string? Note{ get; set; }
        public decimal Total { get; set; }
        public string Address { get; set; }
        public int? CreateByUserId { get; set; }
    }
    public class Order : BaseOrder
    {
        [ForeignKey(nameof(CreateByUserId))]
        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
