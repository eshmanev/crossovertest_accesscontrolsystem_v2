using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using Microsoft.Synchronization;

namespace AccessControl.Contracts.Commands.Synchronization
{
    [ContractClass(typeof(IGetSyncBatchParametersResultContract))]
    public interface IGetSyncBatchParametersResult
    {
        uint BatchSize { get; }

        SyncKnowledge Knowledge { get; }
    }
}