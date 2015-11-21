using System.Diagnostics.Contracts;
using Microsoft.Synchronization;
using Microsoft.Synchronization.MetadataStorage;

namespace AccessControl.Data.Sync.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IMetadataService" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IMetadataService))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IMetadataServiceContract : IMetadataService
    {
        public SyncIdFormatGroup IdFormats
        {
            get
            {
                Contract.Ensures(Contract.Result<SyncIdFormatGroup>() != null);
                return null;
            }
        }

        public SqlMetadataStore OpenStore(out ReplicaMetadata metadata)
        {
            Contract.Ensures(Contract.Result<SqlMetadataStore>() != null);
            Contract.Ensures(metadata != null);
            metadata = null;
            return null;
        }

        public void UpdateMetadata(ReplicaMetadata metadata)
        {
            Contract.Requires(metadata != null);
        }
    }
}