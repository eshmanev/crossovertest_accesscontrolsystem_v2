using System;
using System.Configuration;
using AccessControl.Server.Configuration;
using MassTransit;
using Microsoft.Practices.Unity;

namespace AccessControl.Server.Unity
{
    public class UnityServerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var config = (IServerConfiguration) ConfigurationManager.GetSection("serverConfig");
            var bus = new BusConfiguration(Container, config).Configure();

            Container
                .RegisterInstance(config)
                .RegisterInstance<IBus>(bus)
                .RegisterInstance<IBusControl>(bus);
        }
    }
}