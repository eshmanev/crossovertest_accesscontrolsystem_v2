using System;
using System.Configuration;
using MassTransit;

namespace AccessSimulator
{
    public static class BusHelper
    {
        public static IRequestClient<TRequest, TResponse> CreateClient<TRequest, TResponse>(this IBus bus, string queueName)
            where TRequest : class
            where TResponse : class
        {
            var url = ConfigurationManager.AppSettings["RabbitMqUrl"];
            return new MessageRequestClient<TRequest, TResponse>(bus, new Uri($"{url}/{queueName}"), TimeSpan.FromSeconds(30));
        }
    }
}