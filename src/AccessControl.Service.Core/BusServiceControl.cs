using System.Diagnostics.Contracts;
using MassTransit;
using Topshelf;

namespace AccessControl.Service.Core
{
    public class BusServiceControl : ServiceControl
    {
        private readonly IBusControl _bus;

        public BusServiceControl(IBusControl bus)
        {
            Contract.Requires(bus != null);
            _bus = bus;
        }

        public virtual bool Start(HostControl hostControl)
        {
            _bus.Start();
            return true;
        }

        public virtual bool Stop(HostControl hostControl)
        {
            _bus.Stop();
            return true;
        }
    }
}