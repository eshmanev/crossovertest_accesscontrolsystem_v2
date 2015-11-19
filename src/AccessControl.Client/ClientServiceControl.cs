using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

        public ClientServiceControl(IBusControl busControl, IUnityContainer container)
            : base(busControl)
        {
            Contract.Requires(container != null);
            _container = container;
        }

        public override bool Start(HostControl hostControl)
        {
            _wcfHosts = new[]
            {
                new UnityServiceHost(_container, typeof(AccessCheckService)),
                new UnityServiceHost(_container, typeof(AccessPointRegistry))
            };
            
            _wcfHosts.ForEach(x => x.Open());
            return base.Start(hostControl);
        }

        public override bool Stop(HostControl hostControl)
        {
            _wcfHosts?.ForEach(x => x.Close());
            return base.Stop(hostControl);
        }
    }
}