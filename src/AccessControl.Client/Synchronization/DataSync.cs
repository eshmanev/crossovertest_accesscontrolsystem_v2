using System.Configuration;
using System.Data.SqlServerCe;
using System.Diagnostics;
using AccessControl.Data;
using AccessControl.Data.Configuration;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data.SqlServerCe;

namespace AccessControl.Client.Synchronization
{
    public class DataSync : IDataSync
    {
        private readonly ISessionFactoryHolder _sessionFactoryHolder;
        private readonly IDataConfiguration _dataConfiguration;

        public DataSync(ISessionFactoryHolder sessionFactoryHolder, IDataConfiguration dataConfiguration)
        {
            _sessionFactoryHolder = sessionFactoryHolder;
            _dataConfiguration = dataConfiguration;
        }

        public void Synchronize()
        {
            EnsureDatabase();
            var agent = new SyncOrchestrator
            {
                Direction = SyncDirectionOrder.Download,
                LocalProvider = new SqlCeClientSyncProvider(_dataConfiguration.ConnectionString),
                RemoteProvider = new AccessPointSyncProvider()
            };
            agent.Synchronize();
        }

        private void EnsureDatabase()
        {
            var engine = new SqlCeEngine(_dataConfiguration.ConnectionString);
            engine.CreateDatabase();

            using (var session = _sessionFactoryHolder.SessionFactory.OpenSession())
            {
                Debug.Assert(session.IsOpen);
            }
        }
    }
}