using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Events
{
    /// <summary>
    ///     Occurs when a scheduled group access allowed.
    /// </summary>
    [ContractClass(typeof(IScheduledGroupAccessAllowedContract))]
    public interface IScheduledGroupAccessAllowed
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

        /// <summary>
        ///     Gets the users biometrics.
        /// </summary>
        /// <value>
        ///     The users biometrics.
        /// </value>
        /// <remarks>
        ///     The <see cref="UsersInGroup" /> array correlates the <see cref="UsersBiometrics" /> with indexes.
        /// </remarks>
        string[] UsersBiometrics { get; }

        /// <summary>
        ///     Gets the users in group.
        /// </summary>
        /// <value>
        ///     The users in group.
        /// </value>
        /// <remarks>
        ///     The <see cref="UsersInGroup" /> array correlates the <see cref="UsersBiometrics" /> with indexes.
        /// </remarks>
        string[] UsersInGroup { get; }

        /// <summary>
        ///     Gets the schedule.
        /// </summary>
        /// <value>
        ///     The schedule.
        /// </value>
        IWeeklySchedule WeeklySchedule { get; }
    }
}