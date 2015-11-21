using System.Diagnostics.Contracts;
using AccessControl.Data.Sync.CodeContracts;
using Microsoft.Synchronization;

namespace AccessControl.Data.Sync
{
    /// <summary>
    ///     Represents a holder of replica identifier.
    /// </summary>
    [ContractClass(typeof(IReplicaIdHolderContract))]
    public interface IReplicaIdHolder
    {
        /// <summary>
        ///     Gets the replica identifier.
        /// </summary>
        /// <value>
        ///     The replica identifier.
        /// </value>
        SyncId ReplicaId { get; }
    }
}