using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IScheduledAccessRule" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IScheduledAccessRule))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IScheduledAccessRuleContract : IScheduledAccessRule
    {
        public Guid AccessPointId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);
                return Guid.Empty;
            }
        }

        public TimeSpan FromTimeUtc => default(TimeSpan);

        public TimeSpan ToTimeUtc => default(TimeSpan);
    }
}