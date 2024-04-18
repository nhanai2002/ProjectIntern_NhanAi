using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public class BaseFeedback : BaseClass
    {
        [Key]
        public int FeedbackId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime EvaluateAt { get; set; } = DateTime.Now;
        public int? UserId { get; set; }

    }

    public class Feedback : BaseFeedback
    {
        [Key]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public ICollection<Image>? Images { get; set; }
        public ICollection<ProductFeedBack>? ProductFeedBacks { get; set; }


    }
}
