using System;
using System.Configuration;
using AccessControl.Server.Configuration;
using AccessControl.Server.Data;
using AccessControl.Server.Data.Session;
using MassTransit;
using Microsoft.Practices.Unity;

namespace AccessControl.Server.Unity
{
    public class UnityServerExtension : UnityContainerExtension
    {
        private readonly Func<LifetimeManager> _defaultManager;

        public UnityServerExtension()
        {
            _defaultManager = () => new ContainerControlledLifetimeManager();
        }

        protected override void Initialize()
        {
            var config = (IServerConfiguration) ConfigurationManager.GetSection("serverConfig");
            var bus = new BusConfiguration(Container, config).Configure();

            Container
                .RegisterInstance(config)
                .RegisterInstance<IBus>(bus)
                .RegisterInstance<IBusControl>(bus);

            Container
               .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
               .RegisterType(typeof(IRepository<>), typeof(Repository<>), _defaultManager());
        }
    }
}