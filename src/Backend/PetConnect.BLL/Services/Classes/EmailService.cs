using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    using MailKit.Net.Smtp;
    using MimeKit;
    using PetConnect.BLL.Services.Interfaces;
    using System.Net.Mail;
    using SmtpClient = MailKit.Net.Smtp.SmtpClient;

    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("petconnect.group3@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("petconnect.group3@gmail.com", "teowybdjdgkbgxul");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }

}
