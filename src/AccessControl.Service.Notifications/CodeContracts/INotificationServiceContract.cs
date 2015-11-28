using System;
using System.Diagnostics.Contracts;
using AccessControl.Service.Notifications.Services;

namespace AccessControl.Service.Notifications.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="INotificationService" /> interface.
    /// </summary>
    [ContractClassFor(typeof(INotificationService))]
    // ReSharper disable once InconsistentNaming
    internal abstract class INotificationServiceContract : INotificationService
    {
        public void SendEmail(string emailAddress, string subject, string body)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(emailAddress));
            Contract.Requires(!string.IsNullOrWhiteSpace(subject));
            Contract.Requires(!string.IsNullOrWhiteSpace(body));
        }

        public void SendSms(string number, string body)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(number));
            Contract.Requires(!string.IsNullOrWhiteSpace(body));
        }

        public void UploadCsv(DateTime from, DateTime to)
        {
        }
    }
}