using System;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Service.Configuration;
using AccessControl.Service.Middleware;
using AccessControl.Service.Notifications.Consumers;
using AccessControl.Service.Notifications.Services;
using AccessControl.Service.Security;
using MassTransit;
using MassTransit.QuartzIntegration;
using Microsoft.Practices.Unity;
using Quartz.Impl;

namespace AccessControl.Service.Notifications
{
    public static class Program
    {
        public static ServiceRunner<NotificationServiceControl> CreateService()
        {
            return new ServiceRunner<NotificationServiceControl>()
                .ConfigureContainer(
                    cfg =>
                    {
                        cfg.RegisterRequestClient<IFindUserByName, IFindUserByNameResult>(WellKnownQueues.Ldap)
                           .RegisterRequestClient<IFindUserByBiometrics, IFindUserByBiometricsResult>(WellKnownQueues.AccessControl)
                           .RegisterRequestClient<IFindAccessPointById, IFindAccessPointByIdResult>(WellKnownQueues.AccessControl);

                        var scheduler = new StdSchedulerFactory().GetScheduler();
                        cfg.RegisterInstance(scheduler);
                        cfg.RegisterType<INotificationService, NotificationService>();
                    })
                .ConfigureBus(
                    (cfg, host, container) =>
                    {
                        var rabbitMqConfig = container.Resolve<IServiceConfig>().RabbitMq;
                        cfg.UseMessageScheduler(new Uri(rabbitMqConfig.GetQueueUrl(WellKnownQueues.Notifications)));
                        cfg.UseTickets(container.Resolve<Encryptor>());
                        cfg.ReceiveEndpoint(
                            host,
                            WellKnownQueues.Notifications,
                            e =>
                            {
                                e.Consumer(() => container.Resolve<NotificationConsumer>());
                                e.Consumer(() => container.Resolve<RedeliveryConsumer>());
                                e.Consumer(() => container.Resolve<ScheduleMessageConsumer>());
                                e.Consumer(() => container.Resolve<CancelScheduledMessageConsumer>());
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