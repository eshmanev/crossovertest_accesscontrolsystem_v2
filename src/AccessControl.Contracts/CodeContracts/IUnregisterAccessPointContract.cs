using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUnregisterAccessPoint" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUnregisterAccessPoint))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IUnregisterAccessPointContract : IUnregisterAccessPoint
    {
        public Guid AccessPointId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);
                return Guid.Empty;
            }
        }
    }
}