using Microsoft.Synchronization;

namespace AccessControl.Client.Synchronization
{
    public class AccessControlSyncAgent : SyncAgent
    {

    }

    public class AccessPointSyncProvider : KnowledgeSyncProvider
    {
        public AccessPointSyncProvider()
        {
            
        }
        public override void BeginSession(SyncProviderPosition position, SyncSessionContext syncSessionContext)
        {
            throw new System.NotImplementedException();
        }

        public override void GetSyncBatchParameters(out uint batchSize, out SyncKnowledge knowledge)
        {
            throw new System.NotImplementedException();
        }

        public override ChangeBatch GetChangeBatch(uint batchSize, SyncKnowledge destinationKnowledge, out object changeDataRetriever)
        {
            throw new System.NotImplementedException();
        }

        public override void ProcessChangeBatch(ConflictResolutionPolicy resolutionPolicy, ChangeBatch sourceChanges, object changeDataRetriever, SyncCallbacks syncCallbacks, SyncSessionStatistics sessionStatistics)
        {
            throw new System.NotImplementedException();
        }

        public override FullEnumerationChangeBatch GetFullEnumerationChangeBatch(uint batchSize, SyncId lowerEnumerationBound, SyncKnowledge knowledgeForDataRetrieval, out object changeDataRetriever)
        {
            throw new System.NotImplementedException();
        }

        public override void ProcessFullEnumerationChangeBatch(ConflictResolutionPolicy resolutionPolicy,
                                                               FullEnumerationChangeBatch sourceChanges,
                                                               object changeDataRetriever,
                                                               SyncCallbacks syncCallbacks,
                                                               SyncSessionStatistics sessionStatistics)
        {
            throw new System.NotImplementedException();
        }

        public override void EndSession(SyncSessionContext syncSessionContext)
        {
            throw new System.NotImplementedException();
        }

        public override SyncIdFormatGroup IdFormats { get; }
    }
}