using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IScheduleUserGroupAccess" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IScheduleUserGroupAccess))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IScheduleUserGroupAccessContract : IScheduleUserGroupAccess
    {
        public Guid AccessPointId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);
                return default(Guid);
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

        public ISchedule Schedule
        {
            get
            {
                Contract.Ensures(Contract.Result<ISchedule>() != null);
                return null;
            }
        }
    }
}