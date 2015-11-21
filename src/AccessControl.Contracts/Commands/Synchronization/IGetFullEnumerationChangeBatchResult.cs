using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using Microsoft.Synchronization;

namespace AccessControl.Contracts.Commands.Synchronization
{
    [ContractClass(typeof(IGetFullEnumerationChangeBatchResultContract))]
    public interface IGetFullEnumerationChangeBatchResult
    {
        IChangeDataRetriever ChangeDataRetriever { get; }
        FullEnumerationChangeBatch FullEnumerationChangeBatch { get; }
    }
}