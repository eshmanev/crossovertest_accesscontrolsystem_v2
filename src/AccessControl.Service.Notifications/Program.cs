using AccessControl.Contracts;
using AccessControl.Service.Notifications.Consumers;
using AccessControl.Service.Security;
using MassTransit;
using Microsoft.Practices.Unity;

namespace AccessControl.Service.Notifications
{
    public static class Program
    {
        public static ServiceRunner<BusServiceControl> CreateService()
        {
            return new ServiceRunner()
                .ConfigureContainer(
                    cfg => { })
                .ConfigureBus(
                    (cfg, host, container) =>
                    {
                        cfg.ReceiveEndpoint(
                            host,
                            WellKnownQueues.Notifications,
                            e =>
                            {
                                e.Consumer(() => container.Resolve<AccessConsumer>());
                            });
                    },
                    bus =>
                    {
                        // Cross-services SSO
                        bus.ConnectThreadPrincipal();
                    });
        }

        public static void Main()
        {
            CreateService().Run(
                cfg =>
                {
                    cfg.SetServiceName("AccessControl.Service.Notifications");
                    cfg.SetDescription("Processes notifications");
                });
        }
    }
}