using System;
using System.Collections.Generic;
using AccessControl.Client;
using AccessControl.FunctionalTest.Services;
using AccessControl.Service;
using AccessControl.Service.Notifications;
using AccessControl.Service.Notifications.Services;
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
        private static NotificationServiceControl _notificationService;
        private static readonly List<Action> _cleanupActions = new List<Action>();

        [BeforeFeature]
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

            _notificationService = Run<NotificationServiceControl>(
                ConfigireNotificationService,
                cfg => cfg.SetServiceName("Test.Service.Notification"));

            var builder = new TestBuilder(new WindowsHostEnvironment(new HostConfiguratorImpl()), new WindowsHostSettings());
            _host = (TestHost) builder.Build(
                new TestServiceBuilder(
                                      _accessPointService,
                                      _ldapService,
                                      _notificationService,
                                      _clientService));
            _host.Run();
        }

        [AfterFeature]
        public static void StopServices()
        {
            _accessPointService.Stop(_host);
            _ldapService.Stop(_host);
            _notificationService.Stop(_host);
            _clientService.Stop(_host);
        }

        public static void RegisterCleanup(Action action)
        {
            _cleanupActions.Add(action);
        }

        private static void ConfigireNotificationService(ServiceBuilder<NotificationServiceControl> builder)
        {
            Service.Notifications.Program.ConfigureService(builder);
            builder.ConfigureContainer(container => container.RegisterInstance<INotificationService>(TestNotificationService.Instance));
        }

        private static T Run<T>(Action<ServiceBuilder<T>> configureService, Action<HostConfigurator> configureHost) where T : class, ServiceControl
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