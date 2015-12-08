using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IRemoveGroupSchedule" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IRemoveGroupSchedule))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IRemoveGroupScheduleContract : IRemoveGroupSchedule
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
    }
}