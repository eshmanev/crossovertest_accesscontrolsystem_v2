using System.Diagnostics.Contracts;
using AccessControl.Data.Sync.CodeContracts;
using Microsoft.Synchronization;
using Microsoft.Synchronization.MetadataStorage;

namespace AccessControl.Data.Sync
{
    [ContractClass(typeof(IMetadataServiceContract))]
    public interface IMetadataService
    {
        /// <summary>
        /// Gets the identifier formats.
        /// </summary>
        /// <value>
        /// The identifier formats.
        /// </value>
        SyncIdFormatGroup IdFormats { get; }

        /// <summary>
        /// Opens the metadata store.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns></returns>
        SqlMetadataStore OpenStore(out ReplicaMetadata metadata);

        /// <summary>
        /// Updates the metadata with actual data.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        void UpdateMetadata(ReplicaMetadata metadata);
    }
}