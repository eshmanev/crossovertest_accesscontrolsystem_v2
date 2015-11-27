using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Lists
{
    /// <summary>
    ///     Represents a result of the <see cref="IListLogs" /> command.
    /// </summary>
    [ContractClass(typeof(IListLogsResultContract))]
    public interface IListLogsResult
    {
        /// <summary>
        ///     Gets the logs.
        /// </summary>
        /// <value>
        ///     The logs.
        /// </value>
        ILogEntry[] Logs { get; }
    }
}