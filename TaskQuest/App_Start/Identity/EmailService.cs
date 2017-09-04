using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace TaskQuest.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return SendMail(message);
        }

        // Implementação de e-mail manual
        private Task SendMail(IdentityMessage message)
        {

            var text = HttpUtility.HtmlEncode(message.Body);

            var msg = new MailMessage();
            msg.From = new MailAddress("miyagawa.emanuel@gmail.com", "Eu");
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            msg.AlternateViews.Add(
                AlternateView.CreateAlternateViewFromString(text, System.Text.Encoding.UTF8, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(
                AlternateView.CreateAlternateViewFromString(text, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html));

            var smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            var credentials = new NetworkCredential("miyagawa.emanuel@gmail.com", "0b01t3D34t8G00gl3");
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);


            return Task.FromResult(0);
        }
    }
}