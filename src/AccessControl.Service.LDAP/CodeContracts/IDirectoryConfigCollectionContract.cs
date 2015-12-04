using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.Service.LDAP.Configuration;

namespace AccessControl.Service.LDAP.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IDirectoryConfigCollection" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IDirectoryConfigCollection))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IDirectoryConfigCollectionContract
    {
        public IEnumerable<IDirectoryConfig> Domains
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<IDirectoryConfig>>() != null);
                return null;
            }
        }

        public IDirectoryConfig this[string domain]
        {
            get
            {
                Contract.Requires(!string.IsNullOrWhiteSpace(domain));
                Contract.Ensures(Contract.Result<IDirectoryConfig>() != null);
                return null;
            }
        }
    }
}