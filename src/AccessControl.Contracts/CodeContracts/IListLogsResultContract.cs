using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListLogsResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListLogsResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListLogsResultContract : IListLogsResult
    {
        public ILogEntry[] Logs
        {
            get
            {
                Contract.Ensures(Contract.Result<ILogEntry[]>() != null);
                return null;
            }
        }
    }
}