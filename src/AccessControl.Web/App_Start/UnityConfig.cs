using System;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using AccessControl.Contracts;
using AccessControl.Web.Configuration;
using AccessControl.Web.Models.Account;
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

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            var rabbitMqConfig = (IRabbitMqConfig) WebConfigurationManager.GetSection("rabbitMq");
            container.RegisterInstance(rabbitMqConfig);
            container
                .RegisterType<IUserStore<ApplicationUser>, LdapUserStore>()
                .RegisterType<IAuthenticationManager>(new InjectionFactory(_ => HttpContext.Current.GetOwinContext().Authentication))
                .RegisterType<IRequestClient<IFindUserByName, IFindUserByNameResult>>(new InjectionFactory(_ => CreateRequestClient<IFindUserByName, IFindUserByNameResult>(WellKnownQueues.Ldap)))
                ;

            return container;
        }

        private static IRequestClient<TRequest, TResponse> CreateRequestClient<TRequest, TResponse>(string queueName)
            where TRequest : class
            where TResponse : class
        {
            var bus = Container.Resolve<IBus>();
            var config = Container.Resolve<IRabbitMqConfig>();
            var url = config.Url.EndsWith("/") ? $"{config.Url}{queueName}" : $"{config.Url}/{queueName}";
            return new MessageRequestClient<TRequest, TResponse>(bus, new Uri(url), TimeSpan.FromSeconds(30));
        }
    }
}