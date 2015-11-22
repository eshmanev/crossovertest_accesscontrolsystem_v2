using System.Diagnostics.Contracts;
using AccessControl.Client.Data;
using AccessControl.Client.Services;
using AccessControl.Client.Vendor;
using AccessControl.Service;
using MassTransit;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Topshelf;
using Unity.Wcf;

namespace AccessControl.Client
{
    public class ClientServiceControl : BusServiceControl
    {
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
            Contract.Requires(container != null);
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

            // restore and update permissions
            var service = _container.Resolve<IAccessPermissionService>();
            var accessPermissions = _container.Resolve<IAccessPermissionCollection>();
            service.Load(accessPermissions);
            service.Update(accessPermissions);

            // start WCF services
            _wcfHosts = new[]
            {
                new UnityServiceHost(_container, typeof(AccessCheckService)),
                new UnityServiceHost(_container, typeof(AccessPointRegistry))
            };
            _wcfHosts.ForEach(x => x.Open());

            return result;
        }

        /// <summary>
        ///     Stops the specified host control.
        /// </summary>
        /// <param name="hostControl">The host control.</param>
        /// <returns></returns>
        public override bool Stop(HostControl hostControl)
        {
            // stop WCF services
            _wcfHosts?.ForEach(x => x.Close());

            // stop service bus
            return base.Stop(hostControl);
        }
    }
}