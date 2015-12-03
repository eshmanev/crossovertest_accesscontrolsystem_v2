using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Mail;
using AccessControl.Service.Notifications.Configuration;
using Twilio;

namespace AccessControl.Service.Notifications.Services
{
    internal class NotificationService : INotificationService
    {
        private readonly INotificationConfig _config;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotificationService" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public NotificationService(INotificationConfig config)
        {
            Contract.Requires(config != null);
            _config = config;
        }

        /// <summary>
        ///     Sends an email to the specified email address.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public void SendEmail(string name, string emailAddress, string subject, string body)
        {
            var client = new SmtpClient(_config.Smtp.Host, _config.Smtp.Port)
            {
                Host = _config.Smtp.Host,
                Port = _config.Smtp.Port,
                EnableSsl = _config.Smtp.UseSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_config.Smtp.UserName, _config.Smtp.Password)
            };

            using (var message = new MailMessage(new MailAddress(_config.Smtp.FromAddress, _config.Smtp.senderName), new MailAddress(emailAddress, name)))
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = false;
                client.Send(message);
            }
        }

        /// <summary>
        ///     Sends a short message to the specified phone number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="body">The body.</param>
        public void SendSms(string number, string body)
        {
            var client = new TwilioRestClient(_config.Twilio.AccountSid, _config.Twilio.AuthToken);
            var result = client.SendMessage(_config.Twilio.FromNumber, number, body);
            if (result.RestException != null)
                throw new SmsException(result.RestException);
        }

        public void UploadCsv(DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}