using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseProductCategory : BaseClass
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }

    public class ProductCategory : BaseProductCategory
    {
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
    }

}
