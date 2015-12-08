using System;
using AccessControl.Contracts;
using AccessControl.Web.Configuration;
using MassTransit;
using Microsoft.Practices.Unity;

namespace AccessControl.Web.Services
{
    internal static class BusExtensions
    {
        public static IRequestClient<TRequest, TResponse> LdapClient<TRequest, TResponse>(this IBus bus)
            where TRequest : class
            where TResponse : class
        {
            return GetClient<TRequest, TResponse>(bus, WellKnownQueues.Ldap);
        }

        public static IRequestClient<TRequest, TResponse> AccessControlClient<TRequest, TResponse>(this IBus bus)
          where TRequest : class
          where TResponse : class
        {
            return GetClient<TRequest, TResponse>(bus, WellKnownQueues.AccessControl);
        }

        public static IRequestClient<TRequest, TResponse> GetClient<TRequest, TResponse>(this IBus bus, string queueName)
          where TRequest : class
          where TResponse : class
        {
            var config = UnityConfig.Container.Resolve<IRabbitMqConfig>();
            var url = config.Url.EndsWith("/") ? $"{config.Url}{queueName}" : $"{config.Url}/{queueName}";
            return new MessageRequestClient<TRequest, TResponse>(bus, new Uri(url), TimeSpan.FromSeconds(30));
        }
    }
}