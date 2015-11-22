using System.Diagnostics.Contracts;
using AccessControl.Client.CodeContracts;

namespace AccessControl.Client.Services
{
    /// <summary>
    ///     Provides credentials of the current application.
    /// </summary>
    [ContractClass(typeof(IClientCredentialsContract))]
    internal interface IClientCredentials
    {
        /// <summary>
        ///     Gets the password used by the client for connection.
        /// </summary>
        /// <value>
        ///     The LDAP password.
        /// </value>
        string LdapPassword { get; }

        /// <summary>
        ///     Gets the user name used by the client for connection.
        /// </summary>
        /// <value>
        ///     The name of the LDAP user.
        /// </value>
        string LdapUserName { get; }
    }
}