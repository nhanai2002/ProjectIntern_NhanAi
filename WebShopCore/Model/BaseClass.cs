using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Model
{
    public abstract class BaseClass
    {
        public BaseClass()
        {
            UpdatedAt = DateTime.Now;
            CreatedAt = DateTime.Now;
        }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
