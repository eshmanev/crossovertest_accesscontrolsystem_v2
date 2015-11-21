using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Synchronization;

namespace AccessControl.Data.Sync.Impl
{
    public class RepositoryChangeDataRetriever<T> : IChangeDataRetriever
        where T : class
    {
        private Dictionary<Guid, T> _entities = new Dictionary<Guid, T>();
        

        public RepositoryChangeDataRetriever(ChangeBatchBase changeBatch, ISessionScopeFactory sessionScopeFactory)
        {
            IdFormats = new SyncIdFormatGroup();
            IdFormats.ItemIdFormat.IsVariableLength = false;
            IdFormats.ItemIdFormat.Length = 4;
            IdFormats.ReplicaIdFormat.IsVariableLength = false;
            IdFormats.ReplicaIdFormat.Length = 4;

            foreach (var itemChange in changeBatch)
            {
                if (itemChange.ChangeKind == ChangeKind.Deleted)
                    continue;

                using (var scope = sessionScopeFactory.Create())
                {
                    var id = itemChange.ItemId.GetGuidId();
                    var entity = scope.GetRepository<T>().GetById(id);
                    Debug.Assert(entity != null);
                    _entities.Add(id, entity);
                }
                
            }
        }

        public SyncIdFormatGroup IdFormats { get; }

        public object LoadChangeData(LoadChangeContext loadChangeContext)
        {
            return _entities[loadChangeContext.ItemChange.ItemId.GetGuidId()];
        }
    }
}