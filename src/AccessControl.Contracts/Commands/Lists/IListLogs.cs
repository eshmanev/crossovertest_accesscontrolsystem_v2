using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Lists
{
    /// <summary>
    ///     Lists logs.
    /// </summary>
    [ContractClass(typeof(IListLogsContract))]
    public interface IListLogs
    {
        /// <summary>
        ///     Gets From date, UTC.
        /// </summary>
        /// <value>
        ///     From date, UTC.
        /// </value>
        DateTime FromDateUtc { get; }

        /// <summary>
        ///     Gets To date, UTC.
        /// </summary>
        /// <value>
        ///     To date, UTC.
        /// </value>
        DateTime ToDateUtc { get; }
    }
}