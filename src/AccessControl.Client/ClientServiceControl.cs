using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.Client.Synchronization;
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
        private readonly IDataSync _dataSync;
        private UnityServiceHost[] _wcfHosts;

        public ClientServiceControl(IBusControl busControl, IUnityContainer container, IDataSync dataSync)
            : base(busControl)
        {
            Contract.Requires(container != null);
            _container = container;
            _dataSync = dataSync;
        }

        public override bool Start(HostControl hostControl)
        {
            var result = base.Start(hostControl);

            _wcfHosts = new[]
            {
                new UnityServiceHost(_container, typeof(AccessCheckService)),
                new UnityServiceHost(_container, typeof(AccessPointRegistry))
            };
            
            _wcfHosts.ForEach(x => x.Open());
            _dataSync.Synchronize();

            return result;
        }

        public override bool Stop(HostControl hostControl)
        {
            var result = base.Stop(hostControl);
            _wcfHosts?.ForEach(x => x.Close());
            return result;
        }
    }
}