using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Services.MailService
{
    public interface ISendMailService
    {
        Task<string> SendMail(MailContent mailContent);

    }
}
