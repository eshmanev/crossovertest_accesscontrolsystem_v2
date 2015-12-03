using System.Diagnostics.Contracts;
using AccessControl.Service.Notifications.CodeContracts;

namespace AccessControl.Service.Notifications.Configuration
{
    [ContractClass(typeof(INotificationConfigContract))]
    public interface INotificationConfig
    {
        /// <summary>
        ///     Gets the SMTP settings.
        /// </summary>
        /// <value>
        ///     The SMTP settings.
        /// </value>
        ISmtpSettings Smtp { get; }

        /// <summary>
        ///     Gets the Twilio settings.
        /// </summary>
        /// <value>
        ///     The twilio settings.
        /// </value>
        ITwilioSettings Twilio { get; }

        /// <summary>
        ///     Gets the report settings.
        /// </summary>
        /// <value>
        ///     The report settings.
        /// </value>
        IReportSettings Report { get; }
    }
}