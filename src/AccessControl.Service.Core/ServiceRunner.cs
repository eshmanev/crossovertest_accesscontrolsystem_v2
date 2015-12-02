using System;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Unity;
using MassTransit.Log4NetIntegration;


namespace AccessControl.Service
{
    /// <summary>
    /// Represents a runner of Windows service.
    /// </summary>
    public static class ServiceRunner
    {
        /// <summary>
        /// Runs the service.
        /// </summary>
        /// <typeparam name="T">The type of the service to run.</typeparam>
        /// <param name="configureService">An action used to configure the service environment.</param>
        /// <param name="configureHost">An action used to configure the service host.</param>
        public static void Run<T>(Action<ServiceBuilder<T>> configureService, Action<HostConfigurator> configureHost) where T : class, ServiceControl
        {
            var builder = new ServiceBuilder<T>();
            configureService(builder);
            builder.Build();
            Run(builder, configureHost);
        }

        private static void Run<T>(ServiceBuilder<T> builder, Action<HostConfigurator> config)
            where T : class, ServiceControl
        {
            var tuple = builder.Build();
            HostFactory.Run(
                cfg =>
                {
                    cfg.EnableServiceRecovery(
                        recovery =>
                        {
                            recovery.RestartService(0);
                            recovery.SetResetPeriod(10);
                        });

                    cfg.StartAutomatically();
                    cfg.UseUnityContainer(tuple.Item1);
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