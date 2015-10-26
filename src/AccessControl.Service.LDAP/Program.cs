﻿using System.Configuration;
using AccessControl.Contracts;
using AccessControl.Service.Core;
using AccessControl.Service.LDAP.Configuration;
using AccessControl.Service.LDAP.Consumers;
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
                    })
                .ConfigureBus(
                    (cfg, host, container) =>
                    {
                        cfg.ReceiveEndpoint(
                            host,
                            WellKnownQueues.Ldap,
                            e =>
                            {
                                // e.Durable = false;
                                e.Consumer(() => container.Resolve<FindUserConsumer>());
                                e.Consumer(() => container.Resolve<AuthenticationConsumer>());
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