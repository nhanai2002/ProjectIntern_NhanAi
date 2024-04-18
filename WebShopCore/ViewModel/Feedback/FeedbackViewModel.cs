using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Model;
using WebShopCore.ViewModel.Image;
using WebShopCore.ViewModel.User;

namespace WebShopCore.ViewModel.Feedback
{
    public class FeedbackViewModel : BaseFeedback
    {
        public UserViewModel User { get; set; }
        public List<ImageViewModel>? Images { get; set; }
    }
}
