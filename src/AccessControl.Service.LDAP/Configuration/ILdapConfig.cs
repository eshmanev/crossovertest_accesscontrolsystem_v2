using System.Diagnostics.Contracts;
using AccessControl.Service.LDAP.CodeContracts;

namespace AccessControl.Service.LDAP.Configuration
{
    [ContractClass(typeof(ILdapConfigContract))]
    public interface ILdapConfig
    {
        /// <summary>
        ///     Gets the LDAP server path.
        /// </summary>
        /// <value>
        ///     The LDAP path.
        /// </value>
        string LdapPath { get; }

        /// <summary>
        ///     Gets the LDAP server password.
        /// </summary>
        /// <value>
        ///     The password.
        /// </value>
        string Password { get; }

        /// <summary>
        ///     Gets the LDAP server user name.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }

        /// <summary>
        ///     Combines the LDAP server path with the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        string CombinePath(string path);
    }
}