using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AccessControl.Contracts.Commands.Synchronization;
using Microsoft.Synchronization;

namespace AccessControl.Contracts.Helpers
{
    public class SyncCommand
    {
        public static IBeginSession BeginSession()
        {
            return new BeginSessionImpl();
        }

        public static IEndSession EndSession()
        {
            return new EndSessionImpl();
        }
        public static IGetSyncBatchParameters GetSyncBatchParameters()
        {
            return new GetSyncBatchParametersImpl();
        }

        public static IGetSyncBatchParametersResult GetSyncBatchParametersResult(uint batchSize ,SyncKnowledge knowledge)
        {
            return new GetSyncBatchParametersResultImpl {BatchSize = batchSize, Knowledge = knowledge};
        }

        public static IGetChangeBatch GetChangeBatch(uint batchSize, SyncKnowledge destinationKnowledge)
        {
            return new GetChangeBatchImpl {BatchSize = batchSize, DestinationKnowledge = destinationKnowledge};
        }

        public static IGetChangeBatchResult GetChangeBatchResult(byte[] changeBatch, byte[] changeDataRetriever)
        {
            return new GetChangeBatchResultImpl {ChangeBatch = changeBatch, ChangeDataRetriever = changeDataRetriever};
        }
       
        public static IGetFullEnumerationChangeBatch GetFullEnumerationChangeBatch(uint batchSize, SyncKnowledge knowledgeForDataRetrieval, SyncId lowerEnumerationBound)
        {
            return new GetFullEnumerationChangeBatchImpl {BatchSize = batchSize, KnowledgeForDataRetrieval = knowledgeForDataRetrieval, LowerEnumerationBound = lowerEnumerationBound};
        }

        public static IGetFullEnumerationChangeBatchResult GetFullEnumerationChangeBatchResult(FullEnumerationChangeBatch fullEnumerationChangeBatch, IChangeDataRetriever changeDataRetriever)
        {
            return new GetFullEnumerationChangeBatchResultImpl {ChangeDataRetriever = changeDataRetriever, FullEnumerationChangeBatch = fullEnumerationChangeBatch};
        }

       
       
        #region Nested classes

        private class BeginSessionImpl : IBeginSession
        {
        }

        private class EndSessionImpl : IEndSession
        {
        }

        private class GetSyncBatchParametersImpl : IGetSyncBatchParameters
        {
        }

        private class GetSyncBatchParametersResultImpl : IGetSyncBatchParametersResult
        {
            public uint BatchSize { get; set; }
            public SyncKnowledge Knowledge { get; set; }
        }

        private class GetChangeBatchImpl : IGetChangeBatch
        {
            public uint BatchSize { get; set; }
            public SyncKnowledge DestinationKnowledge { get; set; }
        }

        public class GetChangeBatchResultImpl : IGetChangeBatchResult
        {
            public byte[] ChangeBatch { get; set; }
            public byte[] ChangeDataRetriever { get; set; }
        }

        private class GetFullEnumerationChangeBatchImpl : IGetFullEnumerationChangeBatch
        {
            public uint BatchSize { get; set; }
            public SyncKnowledge KnowledgeForDataRetrieval { get; set; }
            public SyncId LowerEnumerationBound { get; set; }
        }

        private class GetFullEnumerationChangeBatchResultImpl : IGetFullEnumerationChangeBatchResult
        {
            public IChangeDataRetriever ChangeDataRetriever { get; set; }
            public FullEnumerationChangeBatch FullEnumerationChangeBatch { get; set; }
        }

        #endregion
    }
}