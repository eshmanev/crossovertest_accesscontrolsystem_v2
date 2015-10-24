using System;
using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAccessPoint" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAccessPoint))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IAccessPointContract : IAccessPoint
    {
        public Guid AccessPointId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);
                return Guid.Empty;
            }
        }

        public string Description => null;

        public string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}