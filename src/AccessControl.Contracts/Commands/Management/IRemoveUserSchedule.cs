using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Management
{
    /// <summary>
    ///     Removes the weekly schedule for the specified user and access point.
    /// </summary>
    [ContractClass(typeof(IRemoveUserScheduleContract))]
    public interface IRemoveUserSchedule
    {
        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user name.
        /// </summary>
        string UserName { get; }
    }
}