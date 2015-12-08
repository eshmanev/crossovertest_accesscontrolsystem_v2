using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IWeeklySchedule" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IWeeklySchedule))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IWeeklyScheduleContract : IWeeklySchedule
    {
        public string TimeZone
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                Contract.Ensures(TimeZoneInfo.FindSystemTimeZoneById(Contract.Result<string>()) != null);
                return null;
            }
        }

        public IDictionary<DayOfWeek, ITimeRange> DailyTimeRange
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<DayOfWeek, ITimeRange>>() != null);
                return null;
            }
        }
    }
}