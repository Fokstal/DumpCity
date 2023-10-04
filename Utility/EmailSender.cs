using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace DumpCity.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailWorkerSettings _emailWorkerSettings;

        public EmailSender(IConfiguration configuration)
        {
            _emailWorkerSettings = configuration.GetSection("EmailWorker").Get<EmailWorkerSettings>();
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new()
            {
                Port = 587,
                Host = _emailWorkerSettings.SmtpHost! ,
                EnableSsl = true,
                Credentials = new NetworkCredential(_emailWorkerSettings.EmailFrom, _emailWorkerSettings.SmtpKey)
            };

            await client.SendMailAsync(_emailWorkerSettings.EmailFrom!, email, subject, htmlMessage);
        }
    }
}