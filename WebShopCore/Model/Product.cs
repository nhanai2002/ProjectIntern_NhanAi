using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseProduct : BaseClass
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string Code { get; set; }
        [Required]
        public int Status { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public decimal SellPrice { get; set; }        
    }

    public class Product : BaseProduct
    {
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductFeedBack>? ProductFeedBacks { get; set; }
    }

}
