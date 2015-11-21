using System;
using System.Collections.Generic;
using System.IO;
using AccessControl.Data.Entities;
using Microsoft.Synchronization;
using Microsoft.Synchronization.MetadataStorage;

namespace AccessControl.Data.Sync.Impl
{
    public class MetadataService : IMetadataService
    {
        private readonly ISessionScopeFactory _sessionScopeFactory;
        private readonly IReplicaIdHolder _replicaIdHolder;
        private const string MetadataFile = "data.metadata";
        private const string VersionColumn = "VERSION";
        private const string EntityTypeColumn = "ENTITY_TYPE";

        public MetadataService(ISessionScopeFactory sessionScopeFactory, IReplicaIdHolder replicaIdHolder)
        {
            _sessionScopeFactory = sessionScopeFactory;
            _replicaIdHolder = replicaIdHolder;

            IdFormats = new SyncIdFormatGroup();
            IdFormats.ItemIdFormat.IsVariableLength = false;
            IdFormats.ItemIdFormat.Length = 16;
            IdFormats.ReplicaIdFormat.IsVariableLength = false;
            IdFormats.ReplicaIdFormat.Length = 16;
        }

        public SyncIdFormatGroup IdFormats { get; }

        public SqlMetadataStore OpenStore(out ReplicaMetadata metadata)
        {
            SqlMetadataStore metadataStore;

            if (!File.Exists(MetadataFile))
            {
                var fields = new List<FieldSchema>
                {
                    new FieldSchema(VersionColumn, typeof(UInt64)),
                    new FieldSchema(EntityTypeColumn, typeof(string), 50),
                };

                metadataStore = SqlMetadataStore.CreateStore(MetadataFile);
                metadata = metadataStore.InitializeReplicaMetadata(IdFormats, _replicaIdHolder.ReplicaId, fields, null);
            }
            else
            {
               metadataStore = SqlMetadataStore.OpenStore(MetadataFile);
               metadata = metadataStore.GetReplicaMetadata(IdFormats, _replicaIdHolder.ReplicaId);
            }

            metadataStore.BeginTransaction();
            try
            {
                UpdateMetadata(metadata);
                metadataStore.CommitTransaction();
            }
            catch
            {
                metadataStore.RollbackTransaction();
            }

            return metadataStore;
        }

        public void UpdateMetadata(ReplicaMetadata metadata)
        {
            var newVersion = new SyncVersion(0, metadata.GetNextTickCount());
            using (var scope = _sessionScopeFactory.Create())
            {
                var repository = scope.GetRepository<AccessRightsBase>();
                foreach (var entity in repository.GetAll())
                {
                    var syncId = new SyncId(entity.Id);
                    var itemMetadata = metadata.FindItemMetadataById(syncId);

                    if (itemMetadata == null)
                    {
                        // mark new record
                        itemMetadata = metadata.CreateItemMetadata(syncId, newVersion);
                        itemMetadata.ChangeVersion = newVersion;
                        SaveItemMetadata(metadata, itemMetadata, entity);
                    }
                    else
                    {
                        if (entity.Version > itemMetadata.GetUInt64Field(VersionColumn)) // the item has changed since the last sync operation.
                        {
                            itemMetadata.ChangeVersion = newVersion;
                            SaveItemMetadata(metadata, itemMetadata, entity);
                        }
                        else
                        {
                            //Unchanged item, nothing has changes so just mark it as live so that the metadata knows it has not been deleted.
                            metadata.DeleteDetector.ReportLiveItemById(syncId);
                        }
                    }
                }
            }
            

            foreach (var itemMetadata in metadata.DeleteDetector.FindUnreportedItems())
            {
                itemMetadata.MarkAsDeleted(newVersion);
                itemMetadata.SetCustomField(VersionColumn, 0);
                metadata.SaveItemMetadata(itemMetadata);
            }
        }

        private void SaveItemMetadata(ReplicaMetadata metadata, ItemMetadata item, IVersioned entity)
        {
            item.SetCustomField(VersionColumn, entity.Version);
            item.SetCustomField(EntityTypeColumn, entity.GetType().Name);
            metadata.SaveItemMetadata(item);
        }
    }
}