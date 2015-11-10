using System;
using System.Configuration;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;
using AccessControl.Data;
using AccessControl.Data.Configuration;
using AccessControl.Data.Session;
using AccessControl.Service.AccessPoint.Consumers;
using AccessControl.Service.Core;
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
                           .RegisterRequestClient<IFindUserByName, IFindUserByNameResult>(WellKnownQueues.Ldap)
                           .RegisterRequestClient<IFindUsersByDepartment, IFindUsersByDepartmentResult>(WellKnownQueues.Ldap)
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
                                e.Consumer<ListBiometricInfoConsumer>(container);
                                e.Consumer<UpdateUserBiometricConsumer>(container);
                                e.Consumer<AccessRightsConsumer>(container);

                                //e.Consumer(() => new UnityConsumerFactory<AccessPointConsumer>(container));
                                //e.Consumer(() => container.Resolve<ListBiometricInfoConsumer>());
                                //e.Consumer(() => container.Resolve<UpdateUserBiometricConsumer>());
                                //e.Consumer(() => container.Resolve<AccessRightsConsumer>());
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

        public static void Consumer<T>(this IReceiveEndpointConfigurator configurator, IUnityContainer container, Action<IConsumerConfigurator<T>> configure = null)
            where T : class, IConsumer
        {
            var consumerFactory = new UnityConsumerFactory<T>(container);
            configurator.Consumer(consumerFactory, configure);
        }
    }
}