using System.Configuration;

namespace AccessControl.Client.Services
{
    /// <summary>
    ///     Provides credentials of the current application.
    /// </summary>
    internal class ClientCredentials : IClientCredentials
    {
        /// <summary>
        ///     The LDAP password
        /// </summary>
        public string LdapPassword => ConfigurationManager.AppSettings["LDAPPassword"];

        /// <summary>
        ///     Gets or sets the name of the LDAP user.
        /// </summary>
        /// <value>
        ///     The name of the LDAP user.
        /// </value>
        public string LdapUserName => ConfigurationManager.AppSettings["LDAPUserName"];
    }
}