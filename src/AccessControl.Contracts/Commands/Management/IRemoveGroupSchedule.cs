using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Management
{
    /// <summary>
    ///     Removes the weekly schedule for the specified user and access point.
    /// </summary>
    [ContractClass(typeof(IRemoveGroupScheduleContract))]
    public interface IRemoveGroupSchedule
    {
        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the name of the user group.
        /// </summary>
        /// <value>
        ///     The name of the user group.
        /// </value>
        string UserGroupName { get; }
    }
}