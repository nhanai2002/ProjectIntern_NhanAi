using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.ViewModel.Image;

namespace WebShopCore.ViewModel.Product
{
    public class ProductImageViewModel : BaseProductImage
    {
        public ImageViewModel Image { get; set; }
    }
}
