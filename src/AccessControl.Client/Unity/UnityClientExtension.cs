using System.Configuration;
using AccessControl.Client.Configuration;
using AccessControl.Client.Vendor;
using MassTransit;
using Microsoft.Practices.Unity;
using Vendor.API;

namespace AccessControl.Client.Unity
{
    public class UnityClientExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var config = (IClientConfiguration) ConfigurationManager.GetSection("clientConfig");
            var bus = new BusConfiguration(Container, config).Configure();

            Container
                .RegisterInstance(Container)
                .RegisterInstance(config)
                .RegisterInstance<IBus>(bus)
                .RegisterInstance<IBusControl>(bus);

            Container
                .RegisterType<IAccessPointRegistry, AccessPointRegistry>()
                .RegisterType<IAccessCheckService, AccessCheckService>()
                ;
        }
    }
}