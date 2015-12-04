using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Security;

namespace AccessControl.Contracts.Impl.Commands
{
    public class CheckCredentials : ICheckCredentials
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CheckCredentials" /> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public CheckCredentials(string domain, string userName, string password)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(domain));
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(!string.IsNullOrWhiteSpace(password));
            UserName = userName;
            Password = password;
            Domain = domain;
        }

        /// <summary>
        ///     Gets the password.
        /// </summary>
        /// <value>
        ///     The password.
        /// </value>
        public string Password { get; }

        /// <summary>
        ///     Gets the domain.
        /// </summary>
        /// <value>
        ///     The domain.
        /// </value>
        public string Domain { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get; }
    }
}