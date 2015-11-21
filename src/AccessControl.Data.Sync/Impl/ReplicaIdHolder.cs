using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Synchronization;

namespace AccessControl.Data.Sync.Impl
{
    public class ReplicaIdHolder : IReplicaIdHolder
    {
        private readonly Lazy<SyncId> _replicaId;
        private const string ReplicaIdFile = "data.replica";

        public ReplicaIdHolder()
        {
            _replicaId = new Lazy<SyncId>(LoadReplicaId);
        }

        public SyncId ReplicaId => _replicaId.Value;

        private SyncId LoadReplicaId()
        {
            SyncId replicaId;
            if (File.Exists(ReplicaIdFile))
            {
                using (var fs = new FileStream(ReplicaIdFile, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    replicaId = (SyncId) formatter.Deserialize(fs);
                }
            }
            else
            {
                replicaId = new SyncId(Guid.NewGuid());
                using (var fs = new FileStream(ReplicaIdFile, FileMode.Create))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(fs, replicaId);
                }
            }

            return replicaId;
        }
    }
}