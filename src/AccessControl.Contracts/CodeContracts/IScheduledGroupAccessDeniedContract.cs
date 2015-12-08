using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IScheduledGroupAccessDenied" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IScheduledGroupAccessDenied))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IScheduledGroupAccessDeniedContract : IScheduledGroupAccessDenied
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