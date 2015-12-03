using System;
using System.Configuration;

namespace AccessControl.Service.Notifications.Configuration
{
    internal class NotificationConfig : ConfigurationSection, INotificationConfig
    {
        /// <summary>
        ///     Gets the SMTP settings.
        /// </summary>
        /// <value>
        ///     The SMTP settings.
        /// </value>
        [ConfigurationProperty("smtp")]
        public SmtpElement Smtp => (SmtpElement) base["smtp"];

        /// <summary>
        ///     Gets the Twilio settings.
        /// </summary>
        /// <value>
        ///     The twilio settings.
        /// </value>
        [ConfigurationProperty("twilio")]
        public TwilioElement Twilio => (TwilioElement) base["twilio"];

        /// <summary>
        ///     Gets the report settings.
        /// </summary>
        /// <value>
        ///     The report settings.
        /// </value>
        [ConfigurationProperty("report")]
        public ReportElement Report => (ReportElement) base["report"];

        ISmtpSettings INotificationConfig.Smtp => Smtp;

        ITwilioSettings INotificationConfig.Twilio => Twilio;

        IReportSettings INotificationConfig.Report => Report;
    }
}