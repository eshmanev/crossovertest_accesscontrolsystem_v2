using System.Diagnostics.Contracts;
using AccessControl.Service.LDAP.CodeContracts;

namespace AccessControl.Service.LDAP.Configuration
{
    [ContractClass(typeof(ILdapConfigContract))]
    public interface ILdapConfig
    {
        /// <summary>
        ///     Gets the directories.
        /// </summary>
        /// <value>
        ///     The directories.
        /// </value>
        IDirectoryConfigCollection Directories { get; }
    }
}