using System.Configuration;
using AccessControl.Contracts;
using AccessControl.Service.LDAP.Configuration;
using AccessControl.Service.LDAP.Consumers;
using AccessControl.Service.LDAP.Services;
using MassTransit;
using Microsoft.Practices.Unity;

namespace AccessControl.Service.LDAP
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
                        cfg.RegisterType<ILdapService, LdapService>();
                    })
                .ConfigureBus(
                    (cfg, host, container) =>
                    {
                        cfg.ReceiveEndpoint(
                            host,
                            WellKnownQueues.Ldap,
                            e =>
                            {
                                //e.AutoDelete = true;
                                //e.Durable = false;
                                e.Consumer(() => container.Resolve<UserConsumer>());
                                e.Consumer(() => container.Resolve<UserGroupConsumer>());
                                e.Consumer(() => container.Resolve<DepartmentConsumer>());
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
