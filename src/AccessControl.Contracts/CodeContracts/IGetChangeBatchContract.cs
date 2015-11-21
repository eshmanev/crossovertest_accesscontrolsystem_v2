using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Synchronization;
using Microsoft.Synchronization;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IGetChangeBatch" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IGetChangeBatch))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IGetChangeBatchContract : IGetChangeBatch
    {
        public uint BatchSize => 0;

        public SyncKnowledge DestinationKnowledge
        {
            get
            {
                Contract.Ensures(Contract.Result<SyncKnowledge>() != null);
                return null;
            }
        }
    }
}