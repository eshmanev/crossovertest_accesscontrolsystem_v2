using System;
using System.Diagnostics.Contracts;

namespace Vendor.API.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAccessPointRegistry" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAccessPointRegistry))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IAccessPointRegistryContract : IAccessPointRegistry
    {
        public void RegisterAccessPoint(AccessPointDto dto)
        {
            Contract.Requires(dto != null);
        }

        public void UnregisterAccessPoint(Guid accessPointId)
        {
            Contract.Requires(accessPointId != Guid.Empty);
        }
    }
}