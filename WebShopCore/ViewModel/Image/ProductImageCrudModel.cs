using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;

namespace WebShopCore.ViewModel.Image
{
    public class ProductImageCrudModel : BaseProductImage
    {
        public List<IFormFile>? FileImages { get; set; }
        public IFormFile? FileImage { get; set; }
        public int ProductId { get; set; }

    }
}
