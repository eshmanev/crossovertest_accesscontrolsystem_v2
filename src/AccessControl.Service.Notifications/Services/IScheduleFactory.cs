using MassTransit.Scheduling;

namespace AccessControl.Service.Notifications.Services
{
    /// <summary>
    ///     Represents a factory of schedules.
    /// </summary>
    public interface IScheduleFactory
    {
        /// <summary>
        ///     Creates the report schedule.
        /// </summary>
        /// <returns></returns>
        RecurringSchedule CreateReportSchedule();
    }
}