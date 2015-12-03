using System.Diagnostics.Contracts;
using AccessControl.Service.Notifications.Configuration;

namespace AccessControl.Service.Notifications.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="ITwilioSettings" /> interface.
    /// </summary>
    [ContractClassFor(typeof(ITwilioSettings))]
    // ReSharper disable once InconsistentNaming
    internal abstract class ITwilioSettingsContract : ITwilioSettings
    {
        public string AuthToken
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        public string AccountSid
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        public string FromNumber
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }
    }
}