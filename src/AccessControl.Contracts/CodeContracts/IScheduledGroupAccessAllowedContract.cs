using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IScheduledGroupAccessAllowed" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IScheduledGroupAccessAllowed))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IScheduledGroupAccessAllowedContract : IScheduledGroupAccessAllowed
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

        public string[] UsersBiometrics
        {
            get
            {
                Contract.Ensures(Contract.Result<string[]>() != null);
                return null;
            }
        }

        public string[] UsersInGroup
        {
            get
            {
                Contract.Ensures(Contract.Result<string[]>() != null);
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