using System;
using System.Collections.Generic;
using AccessControl.Contracts.Dto;

namespace AccessControl.Web.Models.AccessRights
{
    public class EditAccessRightsViewModel
    {
        /// <summary>
        ///     Gets or sets an array of access points.
        /// </summary>
        public IAccessPoint[] AccessPoints { get; set; }

        /// <summary>
        ///     Gets or sets an array of users.
        /// </summary>
        public IUser[] Users { get; set; }

        /// <summary>
        ///     Gets or sets an array of user groups.
        /// </summary>
        public IUserGroup[] UserGroups { get; set; }

        /// <summary>
        ///     Gets or sets an identifier of the selected access point.
        /// </summary>
        public Guid AccessPointId { get; set; }

        /// <summary>
        ///     Gets or sets a name of the selected user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Gets or sets a name of the selected user group.
        /// </summary>
        public string UserGroupName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether a schedule is applied.
        /// </summary>
        /// <value>
        ///     <c>true</c> if schedule is applied; otherwise, <c>false</c>.
        /// </value>
        public bool ScheduleApplied { get; set; }

        /// <summary>
        ///     Gets or sets the time range per days.
        /// </summary>
        /// <value>
        ///     The time range per days.
        /// </value>
        public Dictionary<string, TimeRange> TimeRangePerDays { get; set; }

        /// <summary>
        ///     Gets or sets the scheduler timezone.
        /// </summary>
        /// <value>
        ///     The scheduler timezone.
        /// </value>
        public string SchedulerTimeZone { get; set; }
    }
}