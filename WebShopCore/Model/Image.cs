using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    }
    public class Image : BaseImage
    {
    }
}
