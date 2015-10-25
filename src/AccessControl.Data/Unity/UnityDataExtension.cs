using System.Configuration;
using AccessControl.Data.Configuration;
using AccessControl.Data.Session;
using Microsoft.Practices.Unity;

namespace AccessControl.Data.Unity
{
    public class UnityDataExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container
                .RegisterInstance((IDataConfiguration)ConfigurationManager.GetSection("dataConfig"))
                .RegisterType<ISessionFactoryHolder, SessionFactoryHolder>(new ContainerControlledLifetimeManager())
                .RegisterType(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}