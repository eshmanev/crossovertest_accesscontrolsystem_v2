

using AccessControl.Service.LDAP.Configuration;

namespace AccessControl.Service.LDAP.CodeContracts
{
    using System.Diagnostics.Contracts;
    using LDAP.Configuration;

    /// <summary>
    /// Represents a contract class for the <see cref="ILdapConfig" /> interface.
    /// </summary>
    [ContractClassFor(typeof(ILdapConfig))]
    // ReSharper disable once InconsistentNaming
    internal abstract class ILdapConfigContract : ILdapConfig
    {
        public string LdapPath
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        public string UserName => null;

        public string Password => null;
        public string CombinePath(string path)
        {
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
            return null;
        }
    }
}