using System;
using System.Configuration;
using AccessControl.Service.Configuration;
using AccessControl.Service;
using AccessControl.Service.Middleware;
using AccessControl.Service.Security;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Practices.Unity;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Unity;

namespace AccessControl.Service
{
    /// <summary>
    ///     Represents a service runner.
    /// </summary>
    public class ServiceRunner : ServiceRunner<BusServiceControl>
    {
    }

    /// <summary>
    ///     Represents a generic service runner.
    /// </summary>
    /// <typeparam name="T">The type of service control.</typeparam>
    public class ServiceRunner<T>
        where T : class, ServiceControl
    {
        private readonly UnityContainer _container;
        private IBusControl _busControl;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceRunner{T}" /> class.
        /// </summary>
        public ServiceRunner()
        {
            _container = new UnityContainer();
            _container.RegisterInstance((IServiceConfig) ConfigurationManager.GetSection("service"));
        }

        /// <summary>
        ///     Configures the bus.
        /// </summary>
        /// <param name="preConfig">The configurator called before the bus is created.</param>
        /// <param name="postConfig">The configurator called after the bus is created.</param>
        /// <returns>This instance.</returns>
        public ServiceRunner<T> ConfigureBus(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost, IUnityContainer> preConfig, Action<IBusControl> postConfig = null)
        {
            var configuration = _container.Resolve<IServiceConfig>();
            _busControl = Bus.Factory.CreateUsingRabbitMq(
                cfg =>
                {
                    cfg.UseExceptionLogger();
                    cfg.UseBsonSerializer();

                    var host = cfg.Host(
                        new Uri(configuration.RabbitMq.Url),
                        h =>
                        {
                            h.Username(configuration.RabbitMq.UserName);
                            h.Password(configuration.RabbitMq.Password);
                        });

                    preConfig(cfg, host, _container);
                });

            postConfig?.Invoke(_busControl);

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