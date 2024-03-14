using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.ViewModel.Category;

namespace WebShopCore.ViewModel.Product
{
    public class ProductCrudModel : BaseProduct
    {
        //public IFormFile? FileImage { get; set; }
        public List<CategoryViewModel>? ListCategoryViewModel { get; set; }
        public int[]? SelectedCategories { get; set; }

    }
}
