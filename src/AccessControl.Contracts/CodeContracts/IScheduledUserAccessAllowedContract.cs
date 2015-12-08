using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IScheduledUserAccessAllowed" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IScheduledUserAccessAllowed))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IScheduledUserAccessAllowedContract : IScheduledUserAccessAllowed
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

        public IWeeklySchedule WeeklySchedule
        {
            get
            {
                Contract.Ensures(Contract.Result<IWeeklySchedule>() != null);
                return null;
            }
        }
    }
}