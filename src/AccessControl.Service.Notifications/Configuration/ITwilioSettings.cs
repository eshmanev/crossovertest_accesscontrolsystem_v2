using System.Diagnostics.Contracts;
using AccessControl.Service.Notifications.CodeContracts;

namespace AccessControl.Service.Notifications.Configuration
{
    [ContractClass(typeof(ITwilioSettingsContract))]
    public interface ITwilioSettings
    {
        /// <summary>
        ///     Gets the twilio authentication token.
        /// </summary>
        /// <value>
        ///     The twilio authentication token.
        /// </value>
        string AuthToken { get; }

        /// <summary>
        ///     Gets the twilio account sid.
        /// </summary>
        /// <value>
        ///     The twilio account sid.
        /// </value>
        string AccountSid { get; }

        /// <summary>
        ///     Gets the twilio from number.
        /// </summary>
        /// <value>
        ///     The twilio from number.
        /// </value>
        string FromNumber { get; }
    }
}