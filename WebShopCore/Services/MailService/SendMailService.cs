﻿using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCore.Services.MailService
{
    public class SendMailService : ISendMailService
    {
        private readonly MailSettings _mailSettings;
        public SendMailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<string> SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);    
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(new MailboxAddress(mailContent.To, mailContent.To));  
            email.Subject = mailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;

            email.Body = builder.ToMessageBody();


            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

                // thuc hien xac thuc
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);

                await smtp.SendAsync(email);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Loi" + e.Message;
            }

            smtp.Disconnect(true);
            return "Gui thanh cong";
        }

    }
}
