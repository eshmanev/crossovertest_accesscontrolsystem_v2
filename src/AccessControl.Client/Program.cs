using System.Configuration;
using AccessControl.Client.Synchronization;
using AccessControl.Client.Vendor;
using AccessControl.Data;
using AccessControl.Data.Configuration;
using AccessControl.Data.Session;
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
            container
                .RegisterInstance((IDataConfiguration) ConfigurationManager.GetSection("dataConfig"), new ContainerControlledLifetimeManager())
                .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager());

            container
                .RegisterType<IAccessPointRegistry, AccessPointRegistry>()
                .RegisterType<IAccessCheckService, AccessCheckService>()
                .RegisterType<IDataSync, DataSync>();
        }
    }
}