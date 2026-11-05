using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SendNotificationAPI.Models.Options;
using SendNotificationAPI.Models.RequestModel;

namespace SendNotificationAPI.Services
{
    public class MailService : INotificationService<SendMailNotificationRequest>
    {
        private readonly SMTPOptions _smtpOptions;

        public MailService(IOptions<SMTPOptions> smtpOptions)
        {
            _smtpOptions = smtpOptions.Value;
        }

        public async Task SendNotificationAsync(SendMailNotificationRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrEmpty(request.Subject) || string.IsNullOrEmpty(request.Body) || request.Recipients == null || request.Recipients.Length == 0)
            {
                throw new ArgumentException("Subject, Body, and Recipients are required");
            }

            using var smtpClient = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port)
            {
                Credentials = new NetworkCredential(_smtpOptions.Username, _smtpOptions.Password),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpOptions.Username),
                Subject = request.Subject,
                Body = request.Body,
                IsBodyHtml = true,
            };

            foreach (var recipient in request.Recipients)
            {
                mailMessage.To.Add(recipient);
            }

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}