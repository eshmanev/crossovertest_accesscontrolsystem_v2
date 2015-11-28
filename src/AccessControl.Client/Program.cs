using AccessControl.Client.Consumers;
using AccessControl.Client.Data;
using AccessControl.Client.Services;
using AccessControl.Client.Vendor;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Service;
using MassTransit;
using Microsoft.Practices.Unity;
using Vendor.API;

namespace AccessControl.Client
{
    public class Program
    {
        public static void Main()
        {
            CreateService().Run(
                cfg =>
                {
                    cfg.SetServiceName("AccessControl.AccessPointClient");
                    cfg.SetDisplayName("Access Point Client");
                    cfg.SetDescription("Represents a glue between Vendor-specific software and Access Control System");
                });
        }

        public static ServiceRunner<ClientServiceControl> CreateService()
        {
            return new ServiceRunner<ClientServiceControl>()
                .ConfigureContainer(
                    cfg =>
                    {
                        // bus
                        cfg
                            .RegisterRequestClient<IListUsersInGroup, IListUsersInGroupResult>(WellKnownQueues.Ldap)
                            .RegisterRequestClient<IAuthenticateUser, IAuthenticateUserResult>(WellKnownQueues.AccessControl)
                            .RegisterRequestClient<IListAccessRights, IListAccessRightsResult>(WellKnownQueues.AccessControl)
                            .RegisterRequestClient<IListUsersBiometric, IListUsersBiometricResult>(WellKnownQueues.AccessControl);

                        // this app
                        cfg
                            .RegisterType<IClientCredentials, ClientCredentials>()
                            .RegisterType<IAccessPermissionCollection, AccessPermissionCollection>(new ContainerControlledLifetimeManager())
                            .RegisterType<IAccessPermissionService, AccessPermissionService>();

                        // vendor
                        cfg.RegisterType<IAccessCheckService, AccessCheckService>();
                    })
                .ConfigureBus(
                    (cfg, host, container) =>
                    {
                        cfg.ReceiveEndpoint(
                            host,
                            "AccessControl.Client",
                            e =>
                            {
                                //e.AutoDelete = true;
                                //e.Durable = false;
                                e.Consumer(() => container.Resolve<EventConsumer>());
                            });
                    });
        }
    }
}