using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Synchronization;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IGetSyncBatchParameters" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IGetSyncBatchParameters))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IGetSyncBatchParametersContract : IGetSyncBatchParameters
    {
    }
}