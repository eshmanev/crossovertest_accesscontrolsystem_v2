using System;
using System.Configuration;
using AccessControl.Service.Configuration;
using AccessControl.Service.Middleware;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransit.Log4NetIntegration.Logging;
using MassTransit.RabbitMqTransport;
using Microsoft.Practices.Unity;
using Topshelf;

namespace AccessControl.Service
{
    /// <summary>
    ///     Represents a service builder.
    /// </summary>
    public class ServiceBuilder : ServiceBuilder<BusServiceControl>
    {
    }

    /// <summary>
    ///     Represents a generic service builder.
    /// </summary>
    /// <typeparam name="T">The type of service control.</typeparam>
    public class ServiceBuilder<T>
        where T : class, ServiceControl
    {
        private readonly UnityContainer _container;
        private IBusControl _busControl;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceBuilder" /> class.
        /// </summary>
        public ServiceBuilder()
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
        public ServiceBuilder<T> ConfigureBus(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost, IUnityContainer> preConfig, Action<IBusControl> postConfig = null)
        {
            var configuration = _container.Resolve<IServiceConfig>();
            
            _busControl = Bus.Factory.CreateUsingRabbitMq(
                cfg =>
                {
                    cfg.UseLog4Net();
                    cfg.UseUnhandledExceptionLogger();
                    cfg.UseJsonSerializer();
                    // binary messages cannot be scheduled
                    // cfg.UseBsonSerializer();

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
        public ServiceBuilder<T> ConfigureContainer(Action<IUnityContainer> config)
        {
            config(_container);
            return this;
        }

        /// <summary>
        /// Builds the configuration.
        /// </summary>
        public Tuple<IUnityContainer, IBusControl> Build()
        {
            if (_busControl == null)
            {
                ConfigureBus((cfg, host, container) => { });
            }

            _container
                .RegisterInstance<IBus>(_busControl)
                .RegisterInstance<IBusControl>(_busControl);

            return Tuple.Create<IUnityContainer, IBusControl>(_container, _busControl);
        }
    }
}