using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WebShopCore.Model
{
    public class BaseProductFeedBack : BaseClass
    {
        [Key]
        public int Id { get; set; }
        public int? FeedbackId { get; set; }
        public int? ProductId { get; set; }
    }

    public class ProductFeedBack : BaseProductFeedBack
    {
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        [ForeignKey(nameof(FeedbackId))]
        public Feedback Feedback { get; set; }

    }
}
