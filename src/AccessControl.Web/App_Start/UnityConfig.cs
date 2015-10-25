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
                .RegisterType(typeof(IRequestClient<,>), new InjectionFactory((c, type, name) => CreateRequestClient(type, rabbitMqConfig)))
                ;

            return container;
        }

        private static object CreateRequestClient(Type type, IRabbitMqConfig config)
        {
            var genericArgs = type.GetGenericArguments();
            var clientType = typeof(MessageRequestClient<,>).MakeGenericType(genericArgs);
            var uri = new Uri(config.Url);
            var bus = Container.Resolve<IBus>();
            var timeout = TimeSpan.FromSeconds(60);
            return Activator.CreateInstance(clientType, bus, uri, timeout);
        }

    }
}