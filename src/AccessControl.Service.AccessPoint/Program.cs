using AccessControl.Contracts;
using AccessControl.Data.Unity;
using AccessControl.Service.AccessPoint.Consumers;
using AccessControl.Service.Core;
using MassTransit;
using Microsoft.Practices.Unity;

namespace AccessControl.Service.AccessPoint
{
    public class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            new ServiceRunner()
                .ConfigureContainer(
                    cfg =>
                    {
                        cfg.AddExtension(new UnityDataExtension());
                        cfg.RegisterRequestClient<IFindUsersByDepartment, IUser[]>(WellKnownQueues.Ldap);
                    })
                .ConfigureBus(
                    (cfg, host, container) =>
                    {
                        cfg.ReceiveEndpoint(
                            host,
                            WellKnownQueues.AccessControl,
                            e =>
                            {
                                e.Consumer(() => container.Resolve<RegisterAccessPointConsumer>());
                                e.Consumer(() => container.Resolve<ListBiometricInfoConsumer>());
                            });
                    })
                .Run(
                    cfg =>
                    {
                        cfg.SetServiceName("AccessControl.AccessPointManager");
                        cfg.SetDisplayName("Access Point Manager");
                        cfg.SetDescription("This service is responsible for access points management");
                    });
        }
    }
}