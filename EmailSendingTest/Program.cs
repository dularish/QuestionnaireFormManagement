using System;
using System.Net;
using System.Net.Mail;

namespace EmailSendingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new SmtpClient("smtp.strato.de", 587) {
                Credentials = new NetworkCredential("dularish.ka@ambigai.net", "somepassword"),
                EnableSsl = true,
            };

            client.Send(new MailMessage("dularish.ka@ambigai.net", "dularish.ka@ambigai.net", "SomeSubject", "SomeBody"));
        }
    }
}
