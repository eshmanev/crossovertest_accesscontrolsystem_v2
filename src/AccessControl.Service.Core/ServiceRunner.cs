using System;
using System.Configuration;
using AccessControl.Service.Core.Configuration;
using AccessControl.Service.Core.Middleware;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Practices.Unity;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Unity;

namespace AccessControl.Service.Core
{
    public class ServiceRunner : ServiceRunner<BusServiceControl>
    {
    }

    public class ServiceRunner<T>
        where T : class, ServiceControl
    {
        private readonly UnityContainer _container;
        private IBusControl _busControl;

        public ServiceRunner()
        {
            _container = new UnityContainer();
            _container.RegisterInstance((IRabbitMqConfig) ConfigurationManager.GetSection("rabbitMq"));
        }

        /// <summary>
        ///     Configures the bus.
        /// </summary>
        /// <param name="config">The configurator.</param>
        /// <returns>This instance.</returns>
        public ServiceRunner<T> ConfigureBus(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost, IUnityContainer> config)
        {
            var rabbitMqConfig = _container.Resolve<IRabbitMqConfig>();
            _busControl = Bus.Factory.CreateUsingRabbitMq(
                cfg =>
                {
                    cfg.UseExceptionLogger();
                    cfg.UseBsonSerializer();
                    // cfg.UseTransaction();

                    var host = cfg.Host(
                        new Uri(rabbitMqConfig.Url),
                        h =>
                        {
                            h.Username(rabbitMqConfig.UserName);
                            h.Password(rabbitMqConfig.Password);
                        });

                    config(cfg, host, _container);
                });
            return this;
        }

        /// <summary>
        ///     Configures the container.
        /// </summary>
        /// <param name="config">The configurator.</param>
        /// <returns>This instance.</returns>
        public ServiceRunner<T> ConfigureContainer(Action<IUnityContainer> config)
        {
            config(_container);
            return this;
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        public void Run(Action<HostConfigurator> config)
        {
            if (_busControl == null)
            {
                ConfigureBus((cfg, host, container) => { });
            }

            _container
                .RegisterInstance<IBus>(_busControl)
                .RegisterInstance<IBusControl>(_busControl);

            HostFactory.Run(
                cfg =>
                {
                    cfg.UseUnityContainer(_container);
                    cfg.Service<T>(
                        s =>
                        {
                            s.ConstructUsingUnityContainer();
                            s.WhenStarted((service, control) => service.Start(control));
                            s.WhenStopped((service, control) => service.Stop(control));
                        });
                    config(cfg);
                });
        }
    }
}