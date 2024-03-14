using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseCategory : BaseClass
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(255, ErrorMessage = "Không được vượt quá 255 ký tự")]
        public string Name { get; set; }

    }

    public class Category : BaseCategory
    {
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }

}
