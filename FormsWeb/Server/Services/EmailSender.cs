using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FormsWeb.Server.Services
{
    public class EmailSender : IEmailSender
    {
        private string _EmailId;
        private string _Password;

        public EmailSender(IConfiguration configuration)
        {
            _EmailId = configuration.GetValue<string>("EmailSenderId");
            _Password = configuration.GetValue<string>("EmailSenderPassword");
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.strato.de", 587)
            {
                Credentials = new NetworkCredential(_EmailId, _Password),
                EnableSsl = true
            };

            return client.SendMailAsync(new MailMessage(_EmailId, email, subject, htmlMessage) { IsBodyHtml = true });
        }
    }
}
