using System.Configuration;
using AccessControl.Contracts;
using AccessControl.LDAP.Service.Configuration;
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
                .ConfigureContainer(
                    cfg =>
                    {
                        cfg.RegisterInstance((ILdapConfig) ConfigurationManager.GetSection("ldap"));
                    })
                .ConfigureBus(
                    (cfg, host, container) =>
                    {
                        cfg.ReceiveEndpoint(
                            host,
                            WellKnownQueues.Ldap,
                            e =>
                            {
                                e.Consumer(() => container.Resolve<FindUserConsumer>());
                                e.Consumer(() => container.Resolve<AuthenticationConsumer>());
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
