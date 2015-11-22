using System.Diagnostics.Contracts;
using AccessControl.Client.Data;
using AccessControl.Client.Services;
using AccessControl.Client.Vendor;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Helpers;
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
            var result = base.Start(hostControl);

            if (!AuthenticateClient())
                return false;

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

        private bool AuthenticateClient()
        {
            var credentials = _container.Resolve<IClientCredentials>();
            var authenticateRequest = _container.Resolve<IRequestClient<IAuthenticateUser, IAuthenticateUserResult>>();
            var result = authenticateRequest.Request(new AuthenticateUser(credentials.LdapUserName, credentials.LdapPassword)).Result;
            if (!result.Authenticated)
                return false;

            // take care of automatical request authentication
            _busControl.ConnectSendObserver(new EncryptedTicketPropagator(result.Ticket));
            return true;
        }

        private void UpdatePermissions()
        {
            var service = _container.Resolve<IAccessPermissionService>();
            var accessPermissions = _container.Resolve<IAccessPermissionCollection>();
            service.Load(accessPermissions);
            service.Update(accessPermissions);
        }

        private void StartWcfServices()
        {
            _wcfHosts = new[]
           {
                new UnityServiceHost(_container, typeof(AccessCheckService)),
                new UnityServiceHost(_container, typeof(AccessPointRegistry))
            };
            _wcfHosts.ForEach(x => x.Open());
        }

        private void StopWcfServices()
        {
            _wcfHosts?.ForEach(x => x.Close());
        }
    }
}