using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.Service.LDAP.CodeContracts;

namespace AccessControl.Service.LDAP.Configuration
{
    [ContractClass(typeof(IDirectoryConfigCollectionContract))]
    public interface IDirectoryConfigCollection : IEnumerable<IDirectoryConfig>
    {
        /// <summary>
        ///     Gets the directory configuration for the specified domain.
        /// </summary>
        /// <value>
        ///     The <see cref="IDirectoryConfig" />.
        /// </value>
        /// <param name="domain">The domain.</param>
        /// <returns></returns>
        IDirectoryConfig this[string domain] { get; }
    }
}