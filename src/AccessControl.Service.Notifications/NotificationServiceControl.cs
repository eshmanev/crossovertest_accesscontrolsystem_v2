using MassTransit;
using MassTransit.QuartzIntegration;
using Quartz;
using Topshelf;

namespace AccessControl.Service.Notifications
{
    /// <summary>
    ///     Represents a notification service control.
    /// </summary>
    public class NotificationServiceControl : BusServiceControl
    {
        private readonly IBusControl _bus;
        private readonly IScheduler _scheduler;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotificationServiceControl" /> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="scheduler">The scheduler.</param>
        public NotificationServiceControl(IBusControl bus, IScheduler scheduler)
            : base(bus)
        {
            _bus = bus;
            _scheduler = scheduler;
        }

        /// <summary>
        ///     Starts the specified host control and internal scheduler.
        /// </summary>
        /// <param name="hostControl">The host control.</param>
        /// <returns></returns>
        public override bool Start(HostControl hostControl)
        {
            try
            {
                _scheduler.JobFactory = new MassTransitJobFactory(_bus);
                _scheduler.Start();
                return base.Start(hostControl);
            }
            catch
            {
                _scheduler.Shutdown();
                throw;
            }
        }

        /// <summary>
        ///     Stops the specified host control and internal scheduler.
        /// </summary>
        /// <param name="hostControl">The host control.</param>
        /// <returns></returns>
        public override bool Stop(HostControl hostControl)
        {
            _scheduler.Standby();
            base.Stop(hostControl);
            _scheduler.Shutdown();
            return true;
        }
    }
}