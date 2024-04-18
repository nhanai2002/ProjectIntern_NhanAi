using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.ViewModel.Product;

namespace WebShopCore.ViewModel.ProductFeedBack
{
    public class FeedbackCrudModel : BaseFeedback
    {
        public int OrderId { get; set; }
        public List<ProductViewModel>? Products { get; set; }
        public List<IFormFile>? FileImages { get; set; }

    }
}
