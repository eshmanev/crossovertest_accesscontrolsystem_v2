using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Timers;
using AccessControl.Client.Data;
using AccessControl.Client.Services;
using AccessControl.Client.Vendor;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Service;
using AccessControl.Service.Configuration;
using AccessControl.Service.Security;
using MassTransit;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Topshelf;
using Unity.Wcf;

namespace AccessControl.Client
{
    public class ClientServiceControl : BusServiceControl
    {
        private readonly Timer _timer = new Timer(TimeSpan.FromMinutes(30).TotalMilliseconds);
        private readonly IRequestClient<Ping, Ping> _ping;
        private readonly IBusControl _busControl;
        private readonly IUnityContainer _container;
        private UnityServiceHost[] _wcfHosts;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientServiceControl" /> class.
        /// </summary>
        /// <param name="busControl">The bus control.</param>
        /// <param name="container">The container.</param>
        public ClientServiceControl(IBusControl busControl, IUnityContainer container)
            : base(busControl)
        {
            Contract.Requires(busControl != null);
            Contract.Requires(container != null);

            _busControl = busControl;
            _container = container;
            var rabbitMqConfig = _container.Resolve<IServiceConfig>().RabbitMq;
            _ping = new MessageRequestClient<Ping, Ping>(
                _busControl,
                new Uri(rabbitMqConfig.GetQueueUrl(WellKnownQueues.AccessControl)),
                TimeSpan.FromSeconds(30));
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is authenticated.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected { get; private set; }

        /// <summary>
        ///     Starts the specified host control.
        /// </summary>
        /// <param name="hostControl">The host control.</param>
        /// <returns></returns>
        public override bool Start(HostControl hostControl)
        {
            // start service bus
            var result = base.Start(hostControl) && StartWcfServices();
            if (result)
            {
                ConnectAsync();
            }
            return result;
        }

        /// <summary>
        ///     Stops the specified host control.
        /// </summary>
        /// <param name="hostControl">The host control.</param>
        /// <returns></returns>
        public override bool Stop(HostControl hostControl)
        {
            var result = StopWcfServices() && base.Stop(hostControl);
            if (result)
                _timer.Stop();
            return result;
        }

        private async void ConnectAsync()
        {
            await AuthenticateClient();
            await UpdatePermissions();
            StartMonitoring();
        }

        private async Task AuthenticateClient()
        {
            var credentials = _container.Resolve<IClientCredentials>();
            var authenticateRequest = _container.Resolve<IRequestClient<IAuthenticateUser, IAuthenticateUserResult>>();

            try
            {
                var result = await authenticateRequest.Request(new AuthenticateUser(credentials.LdapUserName, credentials.LdapPassword));
                if (!result.Authenticated)
                {
                    IsConnected = false;
                    return;
                }

                // take care of automatical request authentication
                _busControl.ConnectTicket(result.Ticket);
                IsConnected = true;
            }
            catch (Exception e)
            {
                LogError("An error occurred while authenticating client", e);
                IsConnected = false;
            }
        }

        private void StartMonitoring()
        {
            _timer.Elapsed += async delegate
                                    {
                                        if (IsConnected)
                                        {
                                            try
                                            {
                                                await _ping.Request(new Ping());
                                            }
                                            catch
                                            {
                                                IsConnected = false;
                                            }
                                        }
                                        else
                                        {
                                            await AuthenticateClient();
                                            await UpdatePermissions();
                                        }
                                    };
            _timer.Start();
        }

        private bool StartWcfServices()
        {
            try
            {
                _wcfHosts = new[]
                {
                    new UnityServiceHost(_container, typeof(AccessCheckService)),
                };
                _wcfHosts.ForEach(x => x.Open());
                return true;
            }
            catch (Exception e)
            {
                LogFatal("An error occurred while starting WCF services", e);
                return false;
            }
        }

        private bool StopWcfServices()
        {
            try
            {
                _wcfHosts?.ForEach(x => x.Close());
                return true;
            }
            catch (Exception e)
            {
                LogFatal("An error occurred while stopping WCF services", e);
                return false;
            }
        }

        private async Task UpdatePermissions()
        {
            var service = _container.Resolve<IAccessPermissionService>();
            var accessPermissions = _container.Resolve<IAccessPermissionCollection>();

            if (!IsConnected)
            {
                // load from cache
                service.Load(accessPermissions);
            }
            else
            {
                // update and save to cache
                await service.Update(accessPermissions);
                service.Save(accessPermissions);
            }
        }
    }
}