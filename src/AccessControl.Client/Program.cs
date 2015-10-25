using AccessControl.Client.Vendor;
using AccessControl.Service.Core;
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
                        cfg.SetServiceName("AccessPointClient");
                        cfg.SetDisplayName("Access Point Client");
                        cfg.SetDescription("Represents a glue between Vendor-specific software and Access Control System");
                    });
        }

        private static void ContainerConfig(IUnityContainer container)
        {
            container
                //.RegisterInstance(container)
                .RegisterType<IAccessPointRegistry, AccessPointRegistry>()
                .RegisterType<IAccessCheckService, AccessCheckService>();
        }
    }
}