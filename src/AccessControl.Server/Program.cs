using AccessControl.Data.Unity;
using AccessControl.Server.Consumers;
using AccessControl.Service.Core;
using MassTransit;
using Microsoft.Practices.Unity;
using Topshelf;
using Topshelf.Unity;

namespace AccessControl.Server
{
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            new ServiceRunner()
                .ConfigureContainer(cfg => cfg.AddExtension(new UnityDataExtension()))
                .ConfigureBus(
                    (cfg, host, container) =>
                    {
                        cfg.ReceiveEndpoint(host, "access_point_queue", e => e.Consumer(() => container.Resolve<RegisterAccessPointConsumer>()));
                    })
                .Run(
                    cfg =>
                    {
                        cfg.SetDisplayName("Access Point Service");
                        cfg.SetDescription("This service is responsible for access points management");
                        cfg.SetServiceName("AccessPointService");
                    });
        }
    }
}