using AccessControl.Contracts;
using AccessControl.Data.Unity;
using AccessControl.Service.AccessPoint.Consumers;
using AccessControl.Service.Core;
using MassTransit;
using Microsoft.Practices.Unity;

namespace AccessControl.Service.AccessPoint
{
    public class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            new ServiceRunner()
                .ConfigureContainer(cfg => cfg.AddExtension(new UnityDataExtension()))
                .ConfigureBus(
                    (cfg, host, container) => { cfg.ReceiveEndpoint(host, WellKnownQueues.AccessPointManager, e => e.Consumer(() => container.Resolve<RegisterAccessPointConsumer>())); })
                .Run(
                    cfg =>
                    {
                        cfg.SetServiceName("AccessControl.AccessPointManager");
                        cfg.SetDisplayName("Access Point Manager");
                        cfg.SetDescription("This service is responsible for access points management");
                    });
        }
    }
}