using System.Diagnostics.Contracts;
using AccessControl.Service.LDAP.Configuration;

namespace AccessControl.Service.LDAP.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="ILdapConfig" /> interface.
    /// </summary>
    [ContractClassFor(typeof(ILdapConfig))]
    // ReSharper disable once InconsistentNaming
    internal abstract class ILdapConfigContract : ILdapConfig
    {
        public IDirectoryConfigCollection Directories => null;
    }
}