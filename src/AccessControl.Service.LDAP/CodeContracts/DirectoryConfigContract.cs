using System.Diagnostics.Contracts;
using AccessControl.Service.LDAP.Configuration;

namespace AccessControl.Service.LDAP.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IDirectoryConfig" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IDirectoryConfig))]
    // ReSharper disable once InconsistentNaming
    internal abstract class DirectoryConfigContract : IDirectoryConfig
    {
        public string DomainName {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        public string Url
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