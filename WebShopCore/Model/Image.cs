using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseImage : BaseClass
    {
        [Key]
        public int ImageId { get;set; }
        public string Url { get;set; }
        
        public int? FeedbackId { get;set; }
    }
    public class Image : BaseImage
    {
        [ForeignKey(nameof(FeedbackId))]
        public Feedback Feedback { get; set; }
    }
}
