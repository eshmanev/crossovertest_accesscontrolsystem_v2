using System.Configuration;
using AccessControl.Client.Configuration;
using AccessControl.Client.Vendor;
using Microsoft.Practices.Unity;
using Vendor.API;

namespace AccessControl.Client.Unity
{
    public class UnityClientExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterInstance(Container);
            Container.RegisterInstance((IClientConfiguration)ConfigurationManager.GetSection("clientConfig"));

            Container
                .RegisterType<IAccessPointRegistry, AccessPointRegistry>()
                .RegisterType<IAccessCheckService, AccessCheckService>()
                ;
        }
    }
}