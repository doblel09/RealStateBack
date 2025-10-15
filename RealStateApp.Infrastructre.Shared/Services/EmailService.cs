using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using RealStateApp.Core.Application.Dtos.Email;
using RealStateApp.Core.Application.Interfaces.Services.Email;
using RealStateApp.Core.Domain.Settings;


namespace RealStateApp.Infrastructre.Shared.Services
{
    public class EmailService : IEmailService
    {
        private MailSettings _mailSettings { get; }
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                MimeMessage email = new();
                email.Sender = MailboxAddress.Parse(_mailSettings.DisplayName + " <" + _mailSettings.EmailFrom + ">");
                email.To.Add(MailboxAddress.Parse(request.To));
                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();
                using MailKit.Net.Smtp.SmtpClient smtp = new();
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
