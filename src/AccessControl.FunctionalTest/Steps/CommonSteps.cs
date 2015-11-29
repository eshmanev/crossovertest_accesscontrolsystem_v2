using System;
using AccessControl.Client;
using AccessControl.Service;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TechTalk.SpecFlow;
using Topshelf;
using Topshelf.Builders;
using Topshelf.HostConfigurators;
using Topshelf.Hosts;
using Topshelf.Runtime;
using Topshelf.Runtime.Windows;
using ServiceBuilder = Topshelf.Builders.ServiceBuilder;

namespace AccessControl.FunctionalTest.Steps
{
    [Binding]
    public class CommonSteps
    {
        private static TestHost _host;
        private static BusServiceControl _accessPointService;
        private static BusServiceControl _ldapService;
        private static ClientServiceControl _clientService;

        [BeforeFeature()]
        public static void StartServices()
        {
            _accessPointService = Run<BusServiceControl>(
               Service.AccessPoint.Program.ConfigureService,
               cfg => cfg.SetServiceName("Test.Service.AccessPoint"));

            _ldapService = Run<BusServiceControl>(
                Service.LDAP.Program.ConfigureService,
                cfg => cfg.SetServiceName("Test.Service.LDAP"));

            _clientService = Run<ClientServiceControl>(
                Client.Program.ConfigureService,
                cfg => cfg.SetServiceName("Test.Service.Client"));

            var builder = new TestBuilder(new WindowsHostEnvironment(new HostConfiguratorImpl()), new WindowsHostSettings());
            _host = (TestHost) builder.Build(
                new TestServiceBuilder(
                                      _accessPointService,
                                      _ldapService,
                                      _clientService));
            _host.Run();
        }

        [AfterFeature()]
        public static void StopServices()
        {
            _accessPointService.Stop(_host);
            _ldapService.Stop(_host);
            _clientService.Stop(_host);
        }

        public static T Run<T>(Action<ServiceBuilder<T>> configureService, Action<HostConfigurator> configureHost) where T : class, ServiceControl
        {
            var builder = new ServiceBuilder<T>();
            configureService(builder);
            var tuple = builder.Build();
            var serviceControl = tuple.Item1.Resolve<T>();
            return serviceControl;
        }

        private class TestServiceBuilder : ServiceBuilder
        {
            private readonly ServiceControl[] _serviceControl;

            public TestServiceBuilder(params ServiceControl[] serviceControl)
            {
                _serviceControl = serviceControl;
            }

            public ServiceHandle Build(HostSettings settings)
            {
                return new TestHandle(_serviceControl);
            }
        }

        private class TestHandle : ServiceHandle
        {
            private readonly ServiceControl[] _serviceControl;

            public TestHandle(params ServiceControl[] serviceControl)
            {
                _serviceControl = serviceControl;
            }

            public void Dispose()
            {
            }

            public bool Start(HostControl hostControl)
            {
                _serviceControl.ForEach(x => x.Start(hostControl));
                return true;
            }

            public bool Pause(HostControl hostControl)
            {
                return false;
            }

            public bool Continue(HostControl hostControl)
            {
                return false;
            }

            public bool Stop(HostControl hostControl)
            {
                return false;
            }

            public void Shutdown(HostControl hostControl)
            {
            }

            public void SessionChanged(HostControl hostControl, SessionChangedArguments arguments)
            {
            }

            public void CustomCommand(HostControl hostControl, int command)
            {
            }
        }
    }
}