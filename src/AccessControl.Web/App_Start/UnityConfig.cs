using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands;
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
                .RegisterRequestClient<IFindUserByName, IFindUserByNameResult>(WellKnownQueues.Ldap)
                .RegisterRequestClient<IAuthenticateUser, IAuthenticateUserResult>(WellKnownQueues.Ldap)
                .RegisterRequestClient<IListUsersExtended, IListUsersExtendedResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IUpdateUserBiometric, IVoidResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IListAccessPoints, IListAccessPointsResult>(WellKnownQueues.AccessControl)
                .RegisterRequestClient<IListDepartments, IListDepartmentsResult>(WellKnownQueues.Ldap)
                .RegisterRequestClient<IRegisterAccessPoint, IVoidResult>(WellKnownQueues.AccessControl)
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
                                var config = container.Resolve<IRabbitMqConfig>();
                                var url = config.Url.EndsWith("/") ? $"{config.Url}{queueName}" : $"{config.Url}/{queueName}";
                                return new MessageRequestClient<TRequest, TResponse>(bus, new Uri(url), TimeSpan.FromSeconds(30));
                            }));
        }
    }
}