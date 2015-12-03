using System.Configuration;

namespace AccessControl.Service.Notifications.Configuration
{
    internal class SmtpElement : ConfigurationElement, ISmtpSettings
    {
        /// <summary>
        ///     The SMTP server
        /// </summary>
        [ConfigurationProperty("host", IsRequired = true)]
        public string Host => (string)base["host"];

        /// <summary>
        ///     Gets or sets the STMP port.
        /// </summary>
        /// <value>
        ///     The STMP port.
        /// </value>
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port => (int)base["port"];

        /// <summary>
        ///     Gets or sets a value indicating whether {CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}[SMTP SSL].
        /// </summary>
        /// <value>
        ///     {D255958A-8513-4226-94B9-080D98F904A1}  <c>true</c> if [SMTP SSL]; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("useSsl", IsRequired = true)]
        public bool UseSsl => (bool)base["useSsl"];

        /// <summary>
        ///     Gets or sets the SMTP user.
        /// </summary>
        /// <value>
        ///     The SMTP user.
        /// </value>
        [ConfigurationProperty("user", IsRequired = true)]
        public string UserName => (string)base["user"];

        /// <summary>
        ///     Gets or sets the SMTP password.
        /// </summary>
        /// <value>
        ///     The SMTP password.
        /// </value>
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password => (string)base["password"];

        /// <summary>
        ///     Gets the SMTP from address.
        /// </summary>
        /// <value>
        ///     The SMTP from address.
        /// </value>
        [ConfigurationProperty("fromAddress", IsRequired = true)]
        public string FromAddress => (string)base["fromAddress"];

        /// <summary>
        ///     Gets the name of the SMTP sender.
        /// </summary>
        /// <value>
        ///     The name of the SMTP sender.
        /// </value>
        [ConfigurationProperty("senderName", IsRequired = true)]
        public string senderName => (string)base["senderName"];
    }
}