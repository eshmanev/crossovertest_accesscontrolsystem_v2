using System.Diagnostics.Contracts;
using AccessControl.Service.LDAP.CodeContracts;

namespace AccessControl.Service.LDAP.Configuration
{
    /// <summary>
    ///     Represents a configuration of LDAP directory.
    /// </summary>
    [ContractClass(typeof(DirectoryConfigContract))]
    public interface IDirectoryConfig
    {
        /// <summary>
        ///     Gets the name of the domain.
        /// </summary>
        /// <value>
        ///     The name of the domain.
        /// </value>
        string DomainName { get; }

        /// <summary>
        ///     Gets the LDAP server url.
        /// </summary>
        /// <value>
        ///     The LDAP url.
        /// </value>
        string Url { get; }

        /// <summary>
        ///     Gets the LDAP server user name.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }

        /// <summary>
        ///     Gets the LDAP server password.
        /// </summary>
        /// <value>
        ///     The password.
        /// </value>
        string Password { get; }

        /// <summary>
        ///     Combines the LDAP server path with the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        string CombinePath(string path);
    }
}