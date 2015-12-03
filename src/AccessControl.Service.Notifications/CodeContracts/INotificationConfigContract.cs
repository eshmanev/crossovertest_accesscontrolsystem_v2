using System.Diagnostics.Contracts;
using AccessControl.Service.Notifications.Configuration;

namespace AccessControl.Service.Notifications.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="INotificationConfig" /> interface.
    /// </summary>
    [ContractClassFor(typeof(INotificationConfig))]
    // ReSharper disable once InconsistentNaming
    internal abstract class INotificationConfigContract : INotificationConfig
    {
        public ISmtpSettings Smtp
        {
            get
            {
                Contract.Ensures(Contract.Result<ISmtpSettings>() != null);
                return null;
            }
        }

        public ITwilioSettings Twilio
        {
            get
            {
                Contract.Ensures(Contract.Result<ITwilioSettings>() != null);
                return null;
            }
        }

        public IReportSettings Report
        {
            get
            {
                Contract.Ensures(Contract.Result<IReportSettings>() != null);
                return null;
            }
        }
    }
}