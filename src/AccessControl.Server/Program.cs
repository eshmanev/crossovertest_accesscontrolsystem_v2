using AccessControl.Server.Unity;
using Microsoft.Practices.Unity;
using Topshelf;
using Topshelf.Unity;

namespace AccessControl.Server
{
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            var container = new UnityContainer();
            container.AddExtension(new UnityServerExtension());

            HostFactory.Run(
                cfg =>
                {
                    cfg.UseUnityContainer(container);
                    cfg.Service<ServerService>(s =>
                    {
                        s.ConstructUsingUnityContainer();
                        s.WhenStarted((service, control) => service.Start(control));
                        s.WhenStopped((service, control) => service.Stop(control));
                    });
                });
        }
    }
}