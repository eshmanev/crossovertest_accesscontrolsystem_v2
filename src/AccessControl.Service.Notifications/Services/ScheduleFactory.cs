using System;
using System.Diagnostics.Contracts;
using AccessControl.Service.Notifications.Configuration;
using MassTransit.Scheduling;

namespace AccessControl.Service.Notifications.Services
{
    public class ScheduleFactory : IScheduleFactory
    {
        private readonly INotificationConfig _notificationConfig;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduleFactory" /> class.
        /// </summary>
        /// <param name="notificationConfig">The notification configuration.</param>
        public ScheduleFactory(INotificationConfig notificationConfig)
        {
            Contract.Requires(notificationConfig != null);
            _notificationConfig = notificationConfig;
        }

        /// <summary>
        ///     Creates the report schedule.
        /// </summary>
        /// <returns></returns>
        public RecurringSchedule CreateReportSchedule()
        {
            return new ReportSchedule
            {
                ScheduleId = "Recurring activity report",
                ScheduleGroup = "Access countrol system",
                TimeZoneId = TimeZoneInfo.Utc.Id,
                StartTime = DateTimeOffset.UtcNow,
                EndTime = null,
                MisfirePolicy = MissedEventPolicy.Send,

                // Every day at the specified time
                // See: http://www.nncron.ru/help/RU/working/cron-format.htm
                CronExpression = $"0 {_notificationConfig.Report.Minutes} {_notificationConfig.Report.Hours} 1/1 * ? *"
            };
        }

        public class ReportSchedule : RecurringSchedule
        {
            /// <summary>
            ///     The timezone of the schedule
            /// </summary>
            public string TimeZoneId { get; set; }

            /// <summary>
            ///     The time the recurring schedule is enabled
            /// </summary>
            public DateTimeOffset StartTime { get; set; }

            /// <summary>
            ///     The time the recurring schedule is disabled If null then the job is repeated forever
            /// </summary>
            public DateTimeOffset? EndTime { get; set; }

            /// <summary>
            ///     A unique name that idenifies this schedule.
            /// </summary>
            public string ScheduleId { get; set; }

            /// <summary>
            ///     A schedule group.
            /// </summary>
            public string ScheduleGroup { get; set; }

            /// <summary>
            ///     The Cron Schedule Expression in Cron Syntax
            /// </summary>
            public string CronExpression { get; set; }

            /// <summary>
            ///     Gets the misfire policy.
            /// </summary>
            /// <value>
            ///     The misfire policy.
            /// </value>
            public MissedEventPolicy MisfirePolicy { get; set; }
        }
    }
}