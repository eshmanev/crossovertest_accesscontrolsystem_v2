using System;
using System.Configuration;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;
using AccessControl.Data;
using AccessControl.Data.Configuration;
using AccessControl.Data.Session;
using AccessControl.Data.Sync;
using AccessControl.Data.Sync.Impl;
using AccessControl.Service.AccessPoint.Consumers;
using AccessControl.Service.AccessPoint.Synchronization;
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
                        // synchronization consumer is statefull
                        cfg.RegisterType<SyncProviderConsumer>(new ContainerControlledLifetimeManager());
                        cfg.RegisterType<IReplicaIdHolder, ReplicaIdHolder>(new ContainerControlledLifetimeManager());
                        cfg.RegisterType<IMetadataService, MetadataService>();

                        // data access
                        cfg.RegisterInstance((IDataConfiguration) ConfigurationManager.GetSection("dataConfig"), new ContainerControlledLifetimeManager())
                           .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
                           .RegisterInstance<ISessionScopeFactory>(cfg.Resolve<SessionFactoryHolder>());

                        // request clients
                        cfg.RegisterRequestClient<IFindUserByName, IFindUserByNameResult>(WellKnownQueues.Ldap)
                           .RegisterRequestClient<IListUsers, IListUsersResult>(WellKnownQueues.Ldap)
                           .RegisterRequestClient<IValidateDepartment, IVoidResult>(WellKnownQueues.Ldap);
                    })
                .ConfigureBus(
                    (cfg, host, container) =>
                    {
                        cfg.ReceiveEndpoint(
                            host,
                            WellKnownQueues.AccessControl,
                            e =>
                            {
                                e.Consumer<AccessPointConsumer>(container);
                                e.Consumer<BiometricInfoConsumer>(container);
                                e.Consumer<AccessRightsConsumer>(container);
                                e.Consumer<SyncProviderConsumer>(container);
                            });
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