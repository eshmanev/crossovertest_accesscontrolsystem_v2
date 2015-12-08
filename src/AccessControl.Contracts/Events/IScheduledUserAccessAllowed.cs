using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Events
{
    /// <summary>
    ///     Occurs when scheduled user access granted.
    /// </summary>
    [ContractClass(typeof(IScheduledUserAccessAllowedContract))]
    public interface IScheduledUserAccessAllowed
    {
        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user's biometric hash.
        /// </summary>
        /// <value>
        ///     The biometric hash.
        /// </value>
        string BiometricHash { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }

        /// <summary>
        ///     Gets the schedule.
        /// </summary>
        /// <value>
        ///     The schedule.
        /// </value>
        IWeeklySchedule WeeklySchedule { get; }
    }
}