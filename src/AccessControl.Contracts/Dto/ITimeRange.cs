using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    [ContractClass(typeof(TimeRangeContract))]
    public interface ITimeRange
    {
        /// <summary>
        ///     Gets FROM time, in UTC.
        /// </summary>
        /// <value>
        ///     FROM time, UTC.
        /// </value>
        TimeSpan FromTime { get; }

        /// <summary>
        ///     Gets TO time, in UTC.
        /// </summary>
        /// <value>
        ///     TO time, UTC.
        /// </value>
        TimeSpan ToTime { get; }
    }
}