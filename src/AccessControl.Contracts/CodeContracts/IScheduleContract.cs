using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="ISchedule" /> interface.
    /// </summary>
    [ContractClassFor(typeof(ISchedule))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IScheduleContract : ISchedule
    {
        public string TimeZone
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                Contract.Ensures(TimeZoneInfo.GetSystemTimeZones().Any(x => x.StandardName == Contract.Result<string>()));
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