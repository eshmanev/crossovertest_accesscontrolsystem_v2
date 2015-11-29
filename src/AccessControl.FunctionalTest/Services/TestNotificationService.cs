using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AccessControl.Service.Notifications.Services;

namespace AccessControl.FunctionalTest.Services
{
    public class TestNotificationService : INotificationService
    {
        private TestNotificationService()
        {
            Emails = new ConcurrentBag<ReceivedEmail>();
            Sms = new ConcurrentBag<ReceivedSms>();
        }

        public static readonly TestNotificationService Instance = new TestNotificationService();

        public ConcurrentBag<ReceivedEmail> Emails { get; }

        public ConcurrentBag<ReceivedSms> Sms { get; }

        public void SendEmail(string emailAddress, string subject, string body)
        {
            Emails.Add(new ReceivedEmail {EmailAddress = emailAddress, Subject = subject, Body = body});
        }

        public void SendSms(string number, string body)
        {
            Sms.Add(new ReceivedSms {Number = number, Body = body});
        }

        public void UploadCsv(DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}