using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="ITimeRange" /> interface.
    /// </summary>
    [ContractClassFor(typeof(ITimeRange))]
    // ReSharper disable once InconsistentNaming
    internal abstract class TimeRangeContract : ITimeRange
    {
        public bool Enabled => false;
        public TimeSpan FromTime => default(TimeSpan);
        public TimeSpan ToTime => default(TimeSpan);
    }
}