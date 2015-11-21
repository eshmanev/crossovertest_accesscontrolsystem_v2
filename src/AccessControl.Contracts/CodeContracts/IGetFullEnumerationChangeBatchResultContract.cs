using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Synchronization;
using Microsoft.Synchronization;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IGetFullEnumerationChangeBatchResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IGetFullEnumerationChangeBatchResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IGetFullEnumerationChangeBatchResultContract : IGetFullEnumerationChangeBatchResult
    {
        public IChangeDataRetriever ChangeDataRetriever
        {
            get
            {
                Contract.Ensures(Contract.Result<IChangeDataRetriever>() != null);
                return null;
            }
        }

        public FullEnumerationChangeBatch FullEnumerationChangeBatch
        {
            get
            {
                Contract.Ensures(Contract.Result<FullEnumerationChangeBatch>() != null);
                return null;
            }
        }
    }
}