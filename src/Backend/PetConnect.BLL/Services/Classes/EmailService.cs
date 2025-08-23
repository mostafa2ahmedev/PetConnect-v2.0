using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Configuration;
    using MimeKit;
    using PetConnect.BLL.Services.Interfaces;
    using System.Net.Mail;
    using SmtpClient = MailKit.Net.Smtp.SmtpClient;

    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body,string? pictureUrl)
        {
            var baseurl = configuration.GetSection("URLS")["base"];

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("petconnect.group3@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder();

            if (!string.IsNullOrEmpty(pictureUrl))
            {
                builder.HtmlBody = $@"
                     <h3>{body}</h3>
                   
                     <img src=""baseurl/{pictureUrl}"" style=""max-width:400px;"">
                 ";
            }
            else
            {
                builder.HtmlBody = body;
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("petconnect.group3@gmail.com", "teowybdjdgkbgxul");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }

}
