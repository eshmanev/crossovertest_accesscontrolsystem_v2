using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.Impl.Events
{
    public class ScheduledUserAccessAllowed : IScheduledUserAccessAllowed
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduledUserAccessAllowed" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="biometricHash">The biometric hash.</param>
        /// <param name="weeklySchedule">The schedule.</param>
        public ScheduledUserAccessAllowed(Guid accessPointId, string userName, string biometricHash, IWeeklySchedule weeklySchedule)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(weeklySchedule != null);
            AccessPointId = accessPointId;
            UserName = userName;
            BiometricHash = biometricHash;
            WeeklySchedule = weeklySchedule;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        public Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user's biometric hash.
        /// </summary>
        /// <value>
        ///     The biometric hash.
        /// </value>
        public string BiometricHash { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get; }

        /// <summary>
        ///     Gets the schedule.
        /// </summary>
        /// <value>
        ///     The schedule.
        /// </value>
        public IWeeklySchedule WeeklySchedule { get; }
    }
}