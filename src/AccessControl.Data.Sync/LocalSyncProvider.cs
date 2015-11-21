using System;
using System.Diagnostics.Contracts;
using System.IO;
using AccessControl.Data.Entities;
using AccessControl.Data.Sync.Impl;
using Microsoft.Synchronization;
using Microsoft.Synchronization.MetadataStorage;

namespace AccessControl.Data.Sync
{
    public class LocalSyncProvider : KnowledgeSyncProvider//, INotifyingChangeApplierTarget
    {
        private readonly IReplicaIdHolder _replicaIdHolder;
        private readonly IMetadataService _metadataService;
        private readonly ISessionScopeFactory _sessionScopeFactory;
        private ReplicaMetadata _metadata;
        private SqlMetadataStore _metadataStore;
        private SyncSessionContext _syncSessionContext;

        public LocalSyncProvider(IReplicaIdHolder replicaIdHolder, IMetadataService metadataService, ISessionScopeFactory sessionScopeFactory)
        {
            Contract.Requires(replicaIdHolder != null);
            Contract.Requires(metadataService != null);

            _replicaIdHolder = replicaIdHolder;
            _metadataService = metadataService;
            _sessionScopeFactory = sessionScopeFactory;
        }

       
        #region Overrides of the KnowledgeSyncProvider

        public override sealed SyncIdFormatGroup IdFormats => _metadataService.IdFormats;

        public override void BeginSession(SyncProviderPosition position, SyncSessionContext syncSessionContext)
        {
            _syncSessionContext = syncSessionContext;
            BeginSession();
        }

        public void BeginSession()
        {
            _metadataStore = _metadataService.OpenStore(out _metadata);
        }

        public override void EndSession(SyncSessionContext syncSessionContext)
        {
            EndSession();
        }

        public void EndSession()
        {
            _metadataStore.Dispose();
            _metadataStore = null;
        }

        public override void GetSyncBatchParameters(out uint batchSize, out SyncKnowledge knowledge)
        {
            batchSize = 10;
            knowledge = _metadata.GetKnowledge();
        }

        public override ChangeBatch GetChangeBatch(uint batchSize, SyncKnowledge destinationKnowledge, out object changeDataRetriever)
        {
            var changeBatch = _metadata.GetChangeBatch(batchSize, destinationKnowledge);
            changeDataRetriever = new RepositoryChangeDataRetriever<AccessRightsBase>(changeBatch, _sessionScopeFactory);
            return changeBatch;
        }

        public override FullEnumerationChangeBatch GetFullEnumerationChangeBatch(uint batchSize, SyncId lowerEnumerationBound, SyncKnowledge knowledgeForDataRetrieval, out object changeDataRetriever)
        {
            var changeBatch = _metadata.GetFullEnumerationChangeBatch(batchSize, lowerEnumerationBound, knowledgeForDataRetrieval);
            changeDataRetriever = new RepositoryChangeDataRetriever<AccessRightsBase>(changeBatch, _sessionScopeFactory);
            return changeBatch;
        }

        public override void ProcessChangeBatch(ConflictResolutionPolicy resolutionPolicy,
                                                ChangeBatch sourceChanges,
                                                object changeDataRetriever,
                                                SyncCallbacks syncCallbacks,
                                                SyncSessionStatistics sessionStatistics)
        {
            //Get all my local change versions from the metadata store
            var localChanges = _metadata.GetLocalVersions(sourceChanges);

            //Create a changeapplier object to make change application easier (make the engine call me 
            //when it needs data and when I should save data)
            var changeApplier = new NotifyingChangeApplier(IdFormats);

            //_metadataStore.BeginTransaction();
            //try
            //{
            //    changeApplier.ApplyChanges(
            //        resolutionPolicy,
            //        sourceChanges,
            //        changeDataRetriever as IChangeDataRetriever,
            //        localChanges,
            //        _metadata.GetKnowledge(),
            //        _metadata.GetForgottenKnowledge(),
            //        this,
            //        _syncSessionContext,
            //        syncCallbacks);

            //    _metadataStore.CommitTransaction();
            //}
            //catch
            //{
            //    _metadataStore.RollbackTransaction();
            //}
        }

        public override void ProcessFullEnumerationChangeBatch(ConflictResolutionPolicy resolutionPolicy,
                                                               FullEnumerationChangeBatch sourceChanges,
                                                               object changeDataRetriever,
                                                               SyncCallbacks syncCallbacks,
                                                               SyncSessionStatistics sessionStatistics)
        {
            //Get all my local change versions from the metadata store
            var localChanges = _metadata.GetFullEnumerationLocalVersions(sourceChanges);

            //Create a changeapplier object to make change application easier (make the engine call me 
            //when it needs data and when I should save data)
            var changeApplier = new NotifyingChangeApplier(IdFormats);

            //_metadataStore.BeginTransaction();
            //try
            //{
            //    changeApplier.ApplyFullEnumerationChanges(
            //        resolutionPolicy,
            //        sourceChanges,
            //        changeDataRetriever as IChangeDataRetriever,
            //        localChanges,
            //        _metadata.GetKnowledge(),
            //        _metadata.GetForgottenKnowledge(),
            //        this,
            //        _syncSessionContext,
            //        syncCallbacks);

            //    _metadataStore.CommitTransaction();
            //}
            //catch
            //{
            //    _metadataStore.RollbackTransaction();
            //}
        }

        #endregion

        /*

        #region INotifyingChangeApplierTarget implementation

        ulong INotifyingChangeApplierTarget.GetNextTickCount()
        {
            return _metadata.GetNextTickCount();
        }

        IChangeDataRetriever INotifyingChangeApplierTarget.GetDataRetriever()
        {
            return _changeDataRetriever;
        }

        bool INotifyingChangeApplierTarget.TryGetDestinationVersion(ItemChange sourceChange, out ItemChange destinationVersion)
        {
            var metadata = _metadata.FindItemMetadataById(sourceChange.ItemId);

            if (metadata == null)
            {
                destinationVersion = null;
                return false;
            }

            destinationVersion = new ItemChange(
                IdFormats,
                _replicaIdHolder.ReplicaId,
                sourceChange.ItemId,
                metadata.IsDeleted ? ChangeKind.Deleted : ChangeKind.Update,
                metadata.CreationVersion,
                metadata.ChangeVersion);

            return true;
        }

        void INotifyingChangeApplierTarget.SaveChangeWithChangeUnits(ItemChange change, SaveChangeWithChangeUnitsContext context)
        {
        }

        void INotifyingChangeApplierTarget.SaveConflict(ItemChange conflictingChange, object conflictingChangeData, SyncKnowledge conflictingChangeKnowledge)
        {
        }

        void INotifyingChangeApplierTarget.StoreKnowledgeForScope(SyncKnowledge knowledge, ForgottenKnowledge forgottenKnowledge)
        {
            _metadata.SetKnowledge(knowledge);
            _metadata.SetForgottenKnowledge(forgottenKnowledge);
            _metadata.SaveReplicaMetadata();
        }

        void INotifyingChangeApplierTarget.SaveItemChange(SaveChangeAction saveChangeAction, ItemChange change, SaveChangeContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        */
    }
}