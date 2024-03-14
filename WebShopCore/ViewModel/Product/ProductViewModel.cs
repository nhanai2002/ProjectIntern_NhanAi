using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.ViewModel.Image;
using WebShopCore.ViewModel.ProductCategory;

namespace WebShopCore.ViewModel.Product
{
    public class ProductViewModel : BaseProduct
    {
        public int Purchases { get; set; }
        public List<ImageViewModel> ListProductImage { get; set; }
        public List<ProductImageViewModel> ProductImages { get; set; }
        public List<ProductCategoryViewModel> ProductCategories { get; set; }


    }
}
