using AccessControl.Client.Unity;
using Microsoft.Practices.Unity;
using Topshelf;
using Topshelf.Unity;

namespace AccessControl.Client
{
    public class Program
    {
        public static void Main()
        {
            var container = new UnityContainer();
            container.AddExtension(new UnityClientExtension());

            HostFactory.Run(
                cfg =>
                {
                    cfg.UseUnityContainer(container);
                    cfg.Service<ClientService>(s =>
                    {
                        s.ConstructUsingUnityContainer();
                        s.WhenStarted((service, control) => service.Start(control));
                        s.WhenStopped((service, control) => service.Stop(control));
                    });
                });
        }
    }
}