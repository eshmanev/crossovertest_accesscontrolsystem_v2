using System;
using System.Diagnostics.Contracts;
using AccessControl.Client.Data;
using AccessControl.Client.Services;
using AccessControl.Client.Vendor;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Service;
using AccessControl.Service.Security;
using log4net;
using MassTransit;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Topshelf;
using Unity.Wcf;

namespace AccessControl.Client
{
    public class ClientServiceControl : BusServiceControl
    {
        private readonly IBusControl _busControl;
        private readonly IUnityContainer _container;
        private UnityServiceHost[] _wcfHosts;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClientServiceControl));

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
        }

        /// <summary>
        ///     Starts the specified host control.
        /// </summary>
        /// <param name="hostControl">The host control.</param>
        /// <returns></returns>
        public override bool Start(HostControl hostControl)
        {
            // start service bus
            var result = base.Start(hostControl);

            AuthenticateClient();
            UpdatePermissions();
            StartWcfServices();

            return result;
        }

        /// <summary>
        ///     Stops the specified host control.
        /// </summary>
        /// <param name="hostControl">The host control.</param>
        /// <returns></returns>
        public override bool Stop(HostControl hostControl)
        {
            StopWcfServices();
            return base.Stop(hostControl);
        }

        private void AuthenticateClient()
        {
            var credentials = _container.Resolve<IClientCredentials>();
            var authenticateRequest = _container.Resolve<IRequestClient<IAuthenticateUser, IAuthenticateUserResult>>();

            try
            {
                var result = authenticateRequest.Request(new AuthenticateUser(credentials.LdapUserName, credentials.LdapPassword)).Result;
                if (!result.Authenticated)
                {
                    return;
                }

                // take care of automatical request authentication
                _busControl.ConnectTicket(result.Ticket);
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while authenticating client", e);
            }
        }

        private void StartWcfServices()
        {
            _wcfHosts = new[]
            {
                new UnityServiceHost(_container, typeof(AccessCheckService)),
            };
            _wcfHosts.ForEach(x => x.Open());
        }

        private void StopWcfServices()
        {
            _wcfHosts?.ForEach(x => x.Close());
        }

        private async void UpdatePermissions()
        {
            var service = _container.Resolve<IAccessPermissionService>();
            var accessPermissions = _container.Resolve<IAccessPermissionCollection>();

            if (await service.Update(accessPermissions))
            {
                // save to cache
                service.Save(accessPermissions);
            }
            else
            {
                // load from cache
                service.Load(accessPermissions);
            }
        }
    }
}