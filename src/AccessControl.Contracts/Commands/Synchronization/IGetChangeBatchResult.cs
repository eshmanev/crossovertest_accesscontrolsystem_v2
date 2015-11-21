using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Synchronization
{
    [ContractClass(typeof(IGetChangeBatchResultContract))]
    public interface IGetChangeBatchResult
    {
        byte[] ChangeBatch { get; }

        byte[] ChangeDataRetriever { get; }
    }
}