using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Client.Data;
using AccessControl.Client.Services;
using AccessControl.Client.Vendor;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Service;
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
        }

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
                ConnectAsync();
            return result;
        }

        /// <summary>
        ///     Stops the specified host control.
        /// </summary>
        /// <param name="hostControl">The host control.</param>
        /// <returns></returns>
        public override bool Stop(HostControl hostControl)
        {
            return StopWcfServices() && base.Stop(hostControl);
        }

        private async void ConnectAsync()
        {
            await AuthenticateClient();
            await UpdatePermissions();
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
                    return;
                }

                // take care of automatical request authentication
                _busControl.ConnectTicket(result.Ticket);
            }
            catch (Exception e)
            {
                LogError("An error occurred while authenticating client", e);
            }
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