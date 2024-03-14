using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;

namespace WebShopCore.ViewModel.Image
{
    public class ImageCrudModel : BaseImage
    {
        public int ProductId { get; set; }
        public List<IFormFile>? FileImages { get; set; }
        public IFormFile? FileImage { get; set; }

    }
}
