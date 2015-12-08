using System;
using System.Linq;
using AccessControl.Contracts.Dto;

namespace AccessControl.Client.Data
{
    public static class ScheduleHelper
    {
        public static bool IsAllowed(this IWeeklySchedule weeklySchedule)
        {
            var now = DateTime.Now;
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(weeklySchedule.TimeZone);
            return weeklySchedule.DailyTimeRange.Any(
                x =>
                {
                    var localFromTime = TimeZoneInfo.ConvertTime(new DateTime(x.Value.FromTime.Ticks), timeZoneInfo, TimeZoneInfo.Local).TimeOfDay;
                    var localToTime = TimeZoneInfo.ConvertTime(new DateTime(x.Value.ToTime.Ticks), timeZoneInfo, TimeZoneInfo.Local).TimeOfDay;
                    return now.DayOfWeek == x.Key && now.TimeOfDay >= localFromTime && now.TimeOfDay <= localToTime;
                });
        } 
    }
}