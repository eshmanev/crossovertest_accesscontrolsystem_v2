using System.Configuration;

namespace AccessControl.Service.Notifications.Configuration
{
    internal class TwilioElement : ConfigurationElement, ITwilioSettings
    {
        /// <summary>
        ///     Gets or sets the twilio authentication token.
        /// </summary>
        /// <value>
        ///     The twilio authentication token.
        /// </value>
        [ConfigurationProperty("authToken", IsRequired = true)]
        public string AuthToken => (string)base["authToken"];

        /// <summary>
        ///     Gets or sets the twilio account sid.
        /// </summary>
        /// <value>
        ///     The twilio account sid.
        /// </value>
        [ConfigurationProperty("accountSid", IsRequired = true)]
        public string AccountSid => (string)base["accountSid"];

        /// <summary>
        ///     Gets or sets the twilio from number.
        /// </summary>
        /// <value>
        ///     The twilio from number.
        /// </value>
        [ConfigurationProperty("fromNumber", IsRequired = true)]
        public string FromNumber => (string)base["fromNumber"];
    }
}