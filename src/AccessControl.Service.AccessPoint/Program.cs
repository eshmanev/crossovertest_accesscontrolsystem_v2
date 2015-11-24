using System;
using System.Configuration;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Dto;
using AccessControl.Data;
using AccessControl.Data.Configuration;
using AccessControl.Data.Session;
using AccessControl.Service.AccessPoint.Consumers;
using AccessControl.Service.Middleware;
using AccessControl.Service.Security;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using Microsoft.Practices.Unity;

namespace AccessControl.Service.AccessPoint
{
    public static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            new ServiceRunner()
                .ConfigureContainer(
                    cfg =>
                    {
                        cfg.RegisterInstance((IDataConfiguration) ConfigurationManager.GetSection("dataConfig"), new ContainerControlledLifetimeManager())
                           .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
                           .RegisterType<IEncryptor, Encryptor>()
                           .RegisterRequestClient<ICheckCredentials, ICheckCredentialsResult>(WellKnownQueues.Ldap)
                           .RegisterRequestClient<IFindUserByName, IFindUserByNameResult>(WellKnownQueues.Ldap)
                           .RegisterRequestClient<IListUsers, IListUsersResult>(WellKnownQueues.Ldap)
                           .RegisterRequestClient<IValidateDepartment, IVoidResult>(WellKnownQueues.Ldap);
                    })
                .ConfigureBus(
                    (cfg, host, container) =>
                    {
                        cfg.UseTickets(container.Resolve<Encryptor>());
                        cfg.ReceiveEndpoint(
                            host,
                            WellKnownQueues.AccessControl,
                            e =>
                            {
                                e.Consumer<AuthenticationConsumer>(container);
                                e.Consumer<AccessPointConsumer>(container);
                                e.Consumer<BiometricInfoConsumer>(container);
                                e.Consumer<AccessRightsConsumer>(container);
                            });
                    },
                    bus =>
                    {
                        // Cross-services SSO
                        bus.ConnectSendObserver(new PrincipalTicketPropagator());
                    })
                .Run(
                    cfg =>
                    {
                        cfg.SetServiceName("AccessControl.AccessPointManager");
                        cfg.SetDisplayName("Access Point Manager");
                        cfg.SetDescription("This service is responsible for access points management");
                    });
        }

        /// <summary>
        /// Configures a consumer to be created withing a unit of work.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="container">The container.</param>
        /// <param name="configure">The configure.</param>
        private static void Consumer<T>(this IReceiveEndpointConfigurator configurator, IUnityContainer container, Action<IConsumerConfigurator<T>> configure = null)
           where T : class, IConsumer
        {
            var consumerFactory = new UnitOfWorkConsumerFactory<T>(container);
            configurator.Consumer(consumerFactory, configure);
        }
    }
}