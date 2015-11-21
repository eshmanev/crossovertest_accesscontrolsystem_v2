using System.Configuration;
using AccessControl.Client.Synchronization;
using AccessControl.Client.Vendor;
using AccessControl.Data;
using AccessControl.Data.Configuration;
using AccessControl.Data.Session;
using AccessControl.Data.Sync;
using AccessControl.Data.Sync.Impl;
using AccessControl.Service;
using Microsoft.Practices.Unity;
using Vendor.API;

namespace AccessControl.Client
{
    public class Program
    {
        public static void Main()
        {
            new ServiceRunner<ClientServiceControl>()
                .ConfigureContainer(ContainerConfig)
                .ConfigureBus(
                    (cfg, host, container) =>
                    {

                    })
                .Run(
                    cfg =>
                    {
                        cfg.SetServiceName("AccessControl.AccessPointClient");
                        cfg.SetDisplayName("Access Point Client");
                        cfg.SetDescription("Represents a glue between Vendor-specific software and Access Control System");
                    });
        }

        private static void ContainerConfig(IUnityContainer container)
        {
            // sync
            container
                .RegisterType<IReplicaIdHolder, ReplicaIdHolder>(new ContainerControlledLifetimeManager())
                .RegisterType<IMetadataService, MetadataService>()
                .RegisterType<IDataSync, DataSync>();

            // data access
            container
                .RegisterInstance((IDataConfiguration) ConfigurationManager.GetSection("dataConfig"), new ContainerControlledLifetimeManager())
                .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
                .RegisterInstance<ISessionScopeFactory>(container.Resolve<SessionFactoryHolder>());

            // vendor
            container
                .RegisterType<IAccessPointRegistry, AccessPointRegistry>()
                .RegisterType<IAccessCheckService, AccessCheckService>();
        }
    }
}