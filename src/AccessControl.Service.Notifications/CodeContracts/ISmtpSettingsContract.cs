using System.Diagnostics.Contracts;
using AccessControl.Service.Notifications.Configuration;

namespace AccessControl.Service.Notifications.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="ISmtpSettings" /> interface.
    /// </summary>
    [ContractClassFor(typeof(ISmtpSettings))]
    // ReSharper disable once InconsistentNaming
    internal abstract class ISmtpSettingsContract : ISmtpSettings
    {
        public string Host
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }
        
        public int Port => 0;

        public bool UseSsl => false;

        public string UserName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        public string Password
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        public string FromAddress
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        public string senderName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }
    }
}