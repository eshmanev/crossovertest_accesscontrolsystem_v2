using System;
using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IRegisterAccessPoint" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IRegisterAccessPoint))]
    // ReSharper disable once InconsistentNaming
    internal abstract class RegisterAccessPointContract : IRegisterAccessPoint
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