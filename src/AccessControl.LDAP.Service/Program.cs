using AccessControl.Contracts;
using AccessControl.LDAP.Service.Consumers;
using AccessControl.Service.Core;
using MassTransit;
using Microsoft.Practices.Unity;

namespace AccessControl.LDAP.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            new ServiceRunner()
                .ConfigureBus(
                    (cfg, host, container) =>
                    {
                        cfg.ReceiveEndpoint(
                            host,
                            WellKnownQueues.Ldap,
                            e =>
                            {
                                e.Consumer(() => container.Resolve<FindUserConsumer>());
                            });
                    })
                .Run(
                    cfg =>
                    {
                        cfg.SetServiceName("AccessControl.LDAPService");
                        cfg.SetDescription("Provides information from LDAP directory");
                    });
        }
    }
}
