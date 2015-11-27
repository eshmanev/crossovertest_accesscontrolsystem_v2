using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IPermanentUserAccessAllowed" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IPermanentUserAccessAllowed))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IPermanentUserAccessAllowedContract : IPermanentUserAccessAllowed
    {
        public Guid AccessPointId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);
                return Guid.Empty;
            }
        }

        public string BiometricHash => null;

        public string UserName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}