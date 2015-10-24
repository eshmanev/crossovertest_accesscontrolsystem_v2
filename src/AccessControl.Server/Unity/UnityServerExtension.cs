using System.Configuration;
using AccessControl.Server.Configuration;
using Microsoft.Practices.Unity;

namespace AccessControl.Server.Unity
{
    public class UnityServerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterInstance((IServerConfiguration)ConfigurationManager.GetSection("serverConfig"));
        }
    }
}