using System;

namespace AccessControl.Service.Notifications.Services
{
    internal class NotificationService : INotificationService
    {
        public void SendEmail(string emailAddress, string subject, string body)
        {
            throw new NotImplementedException();
        }

        public void SendSms(string number, string body)
        {
            throw new NotImplementedException();
        }

        public void UploadCsv(DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}