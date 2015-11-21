using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Synchronization;
using Microsoft.Synchronization;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IGetSyncBatchParametersResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IGetSyncBatchParametersResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IGetSyncBatchParametersResultContract : IGetSyncBatchParametersResult
    {
        public uint BatchSize => 0;

        public SyncKnowledge Knowledge
        {
            get
            {
                Contract.Ensures(Contract.Result<SyncKnowledge>() != null);
                return null;
            }
        }
    }
}