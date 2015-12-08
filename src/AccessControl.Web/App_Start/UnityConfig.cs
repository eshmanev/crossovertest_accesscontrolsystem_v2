using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Dto;
using AccessControl.Web.Configuration;
using AccessControl.Web.Models.Account;
using AccessControl.Web.Services;
using MassTransit;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using Unity.Mvc3;

namespace AccessControl.Web
{
    public static class UnityConfig
    {
        public static IUnityContainer Container { get; private set; }

        public static void ConfigureContainer()
        {
            Container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            var rabbitMqConfig = (IRabbitMqConfig) WebConfigurationManager.GetSection("rabbitMq");
            container.RegisterInstance(rabbitMqConfig);
            container
                .RegisterType<IUserStore<ApplicationUser>, LdapUserStore>()
                .RegisterType<IAuthenticationManager>(new InjectionFactory(_ => HttpContext.Current.GetOwinContext().Authentication))
                .RegisterType<IAccessRightsService, AccessRightsService>()

                // LDAP
                .RegisterRequestClient<IFindUserByName, IFindUserByNameResult>(WellKnownQueues.Ldap)
                .RegisterRequestClient<IListUserGroups, IListUserGroupsResult>(WellKnownQueues.Ldap)
                .RegisterRequestClient<IListDepartments, IListDepartmentsResult>(WellKnownQueues.Ldap)
                .RegisterRequestClient<IListUsers, IListUsersResult>(WellKnownQueues.Ldap)

                // Access Control
                .RegisterRequestClient<IAuthenticateUser, IAuthenticateUserResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IListUsersBiometric, IListUsersBiometricResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IUpdateUserBiometric, IVoidResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IListAccessPoints, IListAccessPointsResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IRegisterAccessPoint, IVoidResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IListAccessRights, IListAccessRightsResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IAllowUserAccess, IVoidResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IAllowUserGroupAccess, IVoidResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IDenyUserAccess, IVoidResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IDenyUserGroupAccess, IVoidResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IListLogs, IListLogsResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IListDelegatedUsers, IListDelegatedUsersResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IGrantManagementRights, IVoidResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IRevokeManagementRights, IVoidResult>(WellKnownQueues.AccessControl)
                ;

            return container;
        }

        private static IUnityContainer RegisterRequestClient<TRequest, TResponse>(this IUnityContainer container, string queueName)
            where TRequest : class
            where TResponse : class
        {
            return
                container
                    .RegisterType<IRequestClient<TRequest, TResponse>>(
                        new InjectionFactory(
                            _ =>
                            {
                                var bus = container.Resolve<IBus>();
                                return bus.GetClient<TRequest, TResponse>(queueName);
                            }));
        }
    }
}