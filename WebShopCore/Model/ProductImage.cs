using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseProductImage : BaseClass
    {
        [Key]
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? ImageId { get; set; }

    }

    public class ProductImage : BaseProductImage
    {
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        [ForeignKey(nameof(ImageId))]
        public Image Image { get; set; }

    }
}
