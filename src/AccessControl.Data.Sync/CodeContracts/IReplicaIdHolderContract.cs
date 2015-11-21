using System.Diagnostics.Contracts;
using Microsoft.Synchronization;

namespace AccessControl.Data.Sync.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IReplicaIdHolder" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IReplicaIdHolder))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IReplicaIdHolderContract : IReplicaIdHolder
    {
        public SyncId ReplicaId
        {
            get
            {
                Contract.Ensures(Contract.Result<SyncId>() != null);
                return null;
            }
        }
    }
}