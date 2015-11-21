using System;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using AccessControl.Data;
using AccessControl.Data.Configuration;
using AccessControl.Data.Sync;
using Microsoft.Synchronization;

namespace AccessControl.Client.Synchronization
{
    public class DataSync : IDataSync
    {
        private readonly IDataConfiguration _dataConfiguration;
        private readonly Func<LocalSyncProvider> _localSyncProviderFactory;
        private readonly Func<RemoteSyncProvider> _remoteSyncProviderFactory;
        private readonly ISessionFactoryHolder _sessionFactoryHolder;

        public DataSync(ISessionFactoryHolder sessionFactoryHolder,
                        IDataConfiguration dataConfiguration,
                        Func<LocalSyncProvider> localSyncProviderFactory,
                        Func<RemoteSyncProvider> remoteSyncProviderFactory)
        {
            Contract.Requires(sessionFactoryHolder != null);
            Contract.Requires(dataConfiguration != null);
            Contract.Requires(localSyncProviderFactory != null);
            Contract.Requires(remoteSyncProviderFactory != null);

            _sessionFactoryHolder = sessionFactoryHolder;
            _dataConfiguration = dataConfiguration;
            _localSyncProviderFactory = localSyncProviderFactory;
            _remoteSyncProviderFactory = remoteSyncProviderFactory;
        }

        public void Synchronize()
        {
            EnsureDatabase();

            var localProvider = _localSyncProviderFactory();
            localProvider.Configuration.ConflictResolutionPolicy = ConflictResolutionPolicy.SourceWins;

            var remoteProvider = _remoteSyncProviderFactory();
            remoteProvider.Configuration.ConflictResolutionPolicy = ConflictResolutionPolicy.DestinationWins;

            var agent = new SyncOrchestrator
            {
                Direction = SyncDirectionOrder.Download,
                LocalProvider = localProvider,
                RemoteProvider = remoteProvider
            };
            
            agent.Synchronize();
        }

        private void EnsureDatabase()
        {
            var engine = new SqlCeEngine(_dataConfiguration.ConnectionString);
            try
            {
                engine.CreateDatabase();
            }
            catch
            {
                // if database file already exists, we should ignore the exception.
            }


            using (var session = _sessionFactoryHolder.SessionFactory.OpenSession())
            {
                Debug.Assert(session.IsOpen);
            }
        }
    }
}