using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.Client.Vendor;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Topshelf;
using Unity.Wcf;

namespace AccessControl.Client
{
    public class ClientService : ServiceControl
    {
        private readonly IUnityContainer _container;
        private UnityServiceHost[] _wcfHosts;

        public ClientService(IUnityContainer container)
        {
            Contract.Requires(container != null);
            _container = container;
        }

        public bool Start(HostControl hostControl)
        {
            _wcfHosts = new[]
            {
                new UnityServiceHost(_container, typeof(AccessCheckService)),
                new UnityServiceHost(_container, typeof(AccessPointRegistry))
            };
            
            _wcfHosts.ForEach(x => x.Open());
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _wcfHosts?.ForEach(x => x.Close());
            return true;
        }
    }
}