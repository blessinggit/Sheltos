using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;

namespace Sheltos.Models.Repositories
{
    public class SmtpEmailSender:IEmailSender
    {
        private readonly IConfiguration _config;

        public SmtpEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            var smtpConfig = _config.GetSection("Smtp");
            var mail = new MailMessage
            {
                From = new MailAddress(smtpConfig["From"]),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mail.To.Add(to);

            using var smtp = new SmtpClient(smtpConfig["Host"], int.Parse(smtpConfig["Port"]))
            {
                Credentials = new NetworkCredential(smtpConfig["User"], smtpConfig["Password"]),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
