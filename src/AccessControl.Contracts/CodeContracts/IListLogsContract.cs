using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Lists;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListLogs" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListLogs))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListLogsContract : IListLogs
    {
        public DateTime FromDateUtc => default(DateTime);
        public DateTime ToDateUtc => default(DateTime);
    }
}