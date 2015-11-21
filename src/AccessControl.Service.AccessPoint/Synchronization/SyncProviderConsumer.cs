using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Synchronization;
using AccessControl.Contracts.Helpers;
using AccessControl.Data.Sync;
using MassTransit;
using Microsoft.Synchronization;

namespace AccessControl.Service.AccessPoint.Synchronization
{
    public class SyncProviderConsumer : IConsumer<IBeginSession>, IConsumer<IEndSession>, IConsumer<IGetChangeBatch>, IConsumer<IGetFullEnumerationChangeBatch>, IConsumer<IGetSyncBatchParameters>
    {
        private readonly LocalSyncProvider _syncProvider;

        public SyncProviderConsumer(LocalSyncProvider syncProvider)
        {
            _syncProvider = syncProvider;
        }

        public Task Consume(ConsumeContext<IBeginSession> context)
        {
            _syncProvider.BeginSession();
            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IEndSession> context)
        {
            _syncProvider.EndSession();
            return Task.FromResult(true);
        }

        public Task Consume(ConsumeContext<IGetChangeBatch> context)
        {
            _syncProvider.BeginSession();
            try
            {
                object changeDataRetriever;
                var changeBatch = _syncProvider.GetChangeBatch(context.Message.BatchSize, context.Message.DestinationKnowledge, out changeDataRetriever);
                var result = SyncCommand.GetChangeBatchResult(
                    SyncHelper.Serialize(changeBatch),
                    SyncHelper.Serialize((IChangeDataRetriever)changeDataRetriever, changeBatch));
                return context.RespondAsync(result);
            }
            finally
            {
                _syncProvider.EndSession();
            }
        }

        public Task Consume(ConsumeContext<IGetFullEnumerationChangeBatch> context)
        {
            object changeDataRetriever;
            var changeBatch = _syncProvider.GetFullEnumerationChangeBatch(context.Message.BatchSize, context.Message.LowerEnumerationBound, context.Message.KnowledgeForDataRetrieval, out changeDataRetriever);
            return context.RespondAsync(SyncCommand.GetFullEnumerationChangeBatchResult(changeBatch, (IChangeDataRetriever) changeDataRetriever));
        }

        public Task Consume(ConsumeContext<IGetSyncBatchParameters> context)
        {
            uint batchSize;
            SyncKnowledge syncKnowledge;
            _syncProvider.GetSyncBatchParameters(out batchSize, out syncKnowledge);
            return context.RespondAsync(SyncCommand.GetSyncBatchParametersResult(batchSize, syncKnowledge));
        }
    }
}