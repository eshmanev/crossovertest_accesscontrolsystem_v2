using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Synchronization;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IGetChangeBatchResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IGetChangeBatchResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IGetChangeBatchResultContract : IGetChangeBatchResult
    {
        public byte[] ChangeBatch
        {
            get
            {
                Contract.Ensures(Contract.Result<byte[]>() != null);
                return null;
            }
        }

        public byte[] ChangeDataRetriever
        {
            get
            {
                Contract.Ensures(Contract.Result<byte[]>() != null);
                return null;
            }
        }
    }
}