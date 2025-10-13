using Microsoft.Extensions.Configuration;
using Store.Services.Helper.Email;
using System.Net;
using System.Net.Mail;

namespace Store.Services.Services.EmailService
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(TempEmail email)
        {
            var smtpSettings = _config.GetSection("SmtpSettings");

            using var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"]))
            {
                EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
                Credentials = new NetworkCredential(
                    smtpSettings["Username"],
                    smtpSettings["Password"]
                )
            };

            var message = new MailMessage
            {
                From = new MailAddress(smtpSettings["SenderEmail"], smtpSettings["SenderName"]),
                Subject = email.Title,
                Body = email.Body,
                IsBodyHtml = true
            };

            message.To.Add(email.To);

            await client.SendMailAsync(message);
        }
    }
}
