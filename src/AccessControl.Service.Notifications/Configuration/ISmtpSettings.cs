using System.Diagnostics.Contracts;
using AccessControl.Service.Notifications.CodeContracts;

namespace AccessControl.Service.Notifications.Configuration
{
    [ContractClass(typeof(ISmtpSettingsContract))]
    public interface ISmtpSettings
    {
        /// <summary>
        ///     Gets the SMTP server.
        /// </summary>
        /// <value>
        ///     The SMTP server.
        /// </value>
        string Host { get; }

        /// <summary>
        ///     Gets a value indicating whether [STMP port].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [STMP port]; otherwise, <c>false</c>.
        /// </value>
        int Port { get; }

        /// <summary>
        ///     Gets a value indicating whether [SMTP SSL].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [SMTP SSL]; otherwise, <c>false</c>.
        /// </value>
        bool UseSsl { get; }

        /// <summary>
        ///     Gets the SMTP user.
        /// </summary>
        /// <value>
        ///     The SMTP user.
        /// </value>
        string UserName { get; }

        /// <summary>
        ///     Gets the SMTP password.
        /// </summary>
        /// <value>
        ///     The SMTP password.
        /// </value>
        string Password { get; }

        /// <summary>
        ///     Gets the SMTP from address.
        /// </summary>
        /// <value>
        ///     The SMTP from address.
        /// </value>
        string FromAddress { get; }

        /// <summary>
        ///     Gets the name of the SMTP sender.
        /// </summary>
        /// <value>
        ///     The name of the SMTP sender.
        /// </value>
        string senderName { get; }
    }
}