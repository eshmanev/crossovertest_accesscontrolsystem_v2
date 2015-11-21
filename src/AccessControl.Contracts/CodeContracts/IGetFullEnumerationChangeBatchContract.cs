using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Synchronization;
using Microsoft.Synchronization;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IGetFullEnumerationChangeBatch" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IGetFullEnumerationChangeBatch))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IGetFullEnumerationChangeBatchContract : IGetFullEnumerationChangeBatch
    {
        public uint BatchSize => 0;

        public SyncKnowledge KnowledgeForDataRetrieval
        {
            get
            {
                Contract.Ensures(Contract.Result<SyncKnowledge>() != null);
                return null;
            }
        }

        public SyncId LowerEnumerationBound
        {
            get
            {
                Contract.Ensures(Contract.Result<SyncId>() != null);
                return null;
            }
        }
    }
}