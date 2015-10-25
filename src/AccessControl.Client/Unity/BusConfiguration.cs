using System;
using System.Diagnostics.Contracts;
using AccessControl.Client.Configuration;
using MassTransit;
using Microsoft.Practices.Unity;

namespace AccessControl.Client.Unity
{
    internal class BusConfiguration
    {
        private readonly IUnityContainer _container;
        private readonly IClientConfiguration _configuration;

        public BusConfiguration(IUnityContainer container, IClientConfiguration configuration)
        {
            Contract.Requires(container != null);
            Contract.Requires(configuration != null);

            _container = container;
            _configuration = configuration;
        }

        public IBusControl Configure()
        {
            return Bus.Factory.CreateUsingRabbitMq(
                cfg =>
                {
                    cfg.UseBsonSerializer();

                    var host = cfg.Host(
                        new Uri(_configuration.RabbitMq.Url),
                        h =>
                        {
                            h.Username(_configuration.RabbitMq.UserName);
                            h.Password(_configuration.RabbitMq.Password);
                        });

                    // cfg.ReceiveEndpoint(host, "access_point_queue", e => e.Consumer<RegisterAccessPointConsumer>());
                });
        }
    }
}