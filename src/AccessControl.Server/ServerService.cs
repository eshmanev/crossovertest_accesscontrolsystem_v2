using System.Diagnostics.Contracts;
using MassTransit;
using Topshelf;

namespace AccessControl.Server
{
    public class ServerService : ServiceControl
    {
        private readonly IBusControl _bus;

        public ServerService(IBusControl bus)
        {
            Contract.Requires(bus != null);
            _bus = bus;
        }

        public bool Start(HostControl hostControl)
        {
            _bus.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _bus.Stop();
            return true;
        }
    }
}