using System;
using System.Diagnostics.Contracts;
using AccessControl.Service.Notifications.CodeContracts;

namespace AccessControl.Service.Notifications.Services
{
    [ContractClass(typeof(INotificationServiceContract))]
    internal interface INotificationService
    {
        /// <summary>
        ///     Sends an email to the specified email address.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        void SendEmail(string emailAddress, string subject, string body);

        /// <summary>
        ///     Sends a short message to the specified phone number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="body">The body.</param>
        void SendSms(string number, string body);

        /// <summary>
        ///     Uploads the CSV report.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        void UploadCsv(DateTime from, DateTime to);
    }
}