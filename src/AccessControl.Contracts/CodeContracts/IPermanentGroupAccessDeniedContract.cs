using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IPermanentGroupAccessDenied" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IPermanentGroupAccessDenied))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IPermanentGroupAccessDeniedContract : IPermanentGroupAccessDenied
    {
        public Guid AccessPointId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);
                return Guid.Empty;
            }
        }

        public string UserGroupName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}