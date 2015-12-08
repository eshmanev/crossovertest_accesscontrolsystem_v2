using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using AccessControl.Contracts.Dto;
using AccessControl.Data.Entities;

namespace AccessControl.Service.AccessPoint.Services
{
    internal static class RuleHelper
    {
        /// <summary>
        ///     Updates the specified rule with the given schedule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="updatedWeeklySchedule">The updated schedule.</param>
        public static void Update(this ScheduledAccessRule rule, IWeeklySchedule updatedWeeklySchedule)
        {
            Contract.Requires(rule != null);
            Contract.Requires(updatedWeeklySchedule != null);

            var source = updatedWeeklySchedule.DailyTimeRange.ToArray().OrderBy(x => x.Key).GetEnumerator();
            var dest = rule.Entries.ToArray().OrderBy(x => x.Day).GetEnumerator();
            while (dest.MoveNext() && source.MoveNext())
            {
                dest.Current.FromTime = source.Current.Value.FromTime;
                dest.Current.ToTime = source.Current.Value.ToTime;
            }

            while (dest.MoveNext())
            {
                rule.RemoveEntry(dest.Current);
            }

            while (source.MoveNext())
            {
                rule.AddEntry(CreateEntry(source.Current));
            }

            rule.TimeZone = updatedWeeklySchedule.TimeZone;
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="SchedulerEntry" />.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static SchedulerEntry CreateEntry(this KeyValuePair<DayOfWeek, ITimeRange> item)
        {
            return new SchedulerEntry
            {
                Day = item.Key,
                FromTime = item.Value.FromTime,
                ToTime = item.Value.ToTime,
            };
        }
    }
}