using System;
using AccessControl.Service.Configuration;
using MassTransit;
using Microsoft.Practices.Unity;

namespace AccessControl.Service
{
    public static class UnityExtensions
    {
        public static IUnityContainer RegisterRequestClient<TRequest, TResponse>(this IUnityContainer container, string queueName)
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
                                var config = container.Resolve<IServiceConfig>();
                                var url = config.RabbitMq.GetQueueUrl(queueName);
                                return new MessageRequestClient<TRequest, TResponse>(bus, new Uri(url), TimeSpan.FromSeconds(30));
                            }));
        }
    }
}