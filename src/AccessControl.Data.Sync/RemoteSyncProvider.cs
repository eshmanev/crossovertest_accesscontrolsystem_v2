using System;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Synchronization;
using AccessControl.Contracts.Helpers;
using AccessControl.Service.Configuration;
using MassTransit;
using Microsoft.Synchronization;

namespace AccessControl.Data.Sync
{
    public class RemoteSyncProvider : KnowledgeSyncProvider
    {
        private readonly IBus _bus;
        private readonly IRabbitMqConfig _mqConfig;
        private SyncSessionContext _syncSessionContext;

        public RemoteSyncProvider(IBus bus, IRabbitMqConfig mqConfig)
        {
            _bus = bus;
            _mqConfig = mqConfig;
            IdFormats = new SyncIdFormatGroup();
            IdFormats.ItemIdFormat.IsVariableLength = false;
            IdFormats.ItemIdFormat.Length = 16;
            IdFormats.ReplicaIdFormat.IsVariableLength = false;
            IdFormats.ReplicaIdFormat.Length = 16;
        }

        public override sealed SyncIdFormatGroup IdFormats { get; }

        public override async void BeginSession(SyncProviderPosition position, SyncSessionContext syncSessionContext)
        {
            //_syncSessionContext = syncSessionContext;
            //await _bus.Publish(SyncCommand.BeginSession());
        }

        public override async void EndSession(SyncSessionContext syncSessionContext)
        {
            //_syncSessionContext = null;
            //await _bus.Publish(SyncCommand.EndSession());
        }

        public override ChangeBatch GetChangeBatch(uint batchSize, SyncKnowledge destinationKnowledge, out object changeDataRetriever)
        {
            var task = Request<IGetChangeBatch, IGetChangeBatchResult>(SyncCommand.GetChangeBatch(batchSize, destinationKnowledge));
            var result = task.Result;
            return result.Read(out changeDataRetriever);
        }

        public override FullEnumerationChangeBatch GetFullEnumerationChangeBatch(uint batchSize, SyncId lowerEnumerationBound, SyncKnowledge knowledgeForDataRetrieval, out object changeDataRetriever)
        {
            var task = Request<IGetFullEnumerationChangeBatch, IGetFullEnumerationChangeBatchResult>(SyncCommand.GetFullEnumerationChangeBatch(batchSize, knowledgeForDataRetrieval, lowerEnumerationBound));
            var result = task.Result;
            changeDataRetriever = result.ChangeDataRetriever;
            return result.FullEnumerationChangeBatch;
        }

        public override void GetSyncBatchParameters(out uint batchSize, out SyncKnowledge knowledge)
        {
            var task = Request<IGetSyncBatchParameters, IGetSyncBatchParametersResult>(SyncCommand.GetSyncBatchParameters());
            var result = task.Result;
            batchSize = result.BatchSize;
            knowledge = result.Knowledge;
        }

        public override void ProcessChangeBatch(ConflictResolutionPolicy resolutionPolicy,
                                                ChangeBatch sourceChanges,
                                                object changeDataRetriever,
                                                SyncCallbacks syncCallbacks,
                                                SyncSessionStatistics sessionStatistics)
        {
            // one-way sync only
            throw new NotSupportedException();
        }

        public override void ProcessFullEnumerationChangeBatch(ConflictResolutionPolicy resolutionPolicy,
                                                               FullEnumerationChangeBatch sourceChanges,
                                                               object changeDataRetriever,
                                                               SyncCallbacks syncCallbacks,
                                                               SyncSessionStatistics sessionStatistics)
        {
            // one-way sync only
            throw new NotSupportedException();
        }

        private async Task<TResponse> Request<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            var uri = new Uri(_mqConfig.GetQueueUrl(WellKnownQueues.AccessControl));
            Task<TResponse> responseTask = null;
            await _bus.Request(
                uri,
                request,
                x =>
                {
                    x.Timeout = TimeSpan.FromSeconds(30);
                    responseTask = x.Handle<TResponse>();
                },
                CancellationToken.None);

            return await responseTask;
        }
    }
}