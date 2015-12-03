using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Service.Configuration;
using AccessControl.Service.Notifications.Consumers;
using AccessControl.Service.Notifications.Messages;
using AccessControl.Service.Notifications.Services;
using MassTransit;
using MassTransit.Pipeline;
using MassTransit.QuartzIntegration;
using MassTransit.Scheduling;
using Newtonsoft.Json;
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
        private readonly IServiceConfig _serviceConfig;
        private readonly IScheduleFactory _scheduleFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotificationServiceControl" /> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="serviceConfig">The service configuration.</param>
        /// <param name="scheduleFactory">The schedule factory.</param>
        public NotificationServiceControl(IBusControl bus, IScheduler scheduler, IServiceConfig serviceConfig, IScheduleFactory scheduleFactory)
            : base(bus)
        {
            Contract.Requires(bus != null);
            Contract.Requires(scheduler != null);
            Contract.Requires(serviceConfig != null);
            Contract.Requires(scheduleFactory != null);

            _bus = bus;
            _scheduler = scheduler;
            _serviceConfig = serviceConfig;
            _scheduleFactory = scheduleFactory;
        }

        /// <summary>
        ///     Starts the specified host control and internal scheduler.
        /// </summary>
        /// <param name="hostControl">The host control.</param>
        /// <returns></returns>
        public override bool Start(HostControl hostControl)
        {
            if (!base.Start(hostControl))
            {
                return false;
            }

            try
            {
                _scheduler.JobFactory = new MassTransitJobFactory(_bus);
                _scheduler.Start();

                var schedule = _scheduleFactory.CreateReportSchedule();
                var scheduleMessage = new GenerateDailyReport();
                var schedulePipe = Pipe.Execute<PublishContext<ScheduleRecurringMessage<GenerateDailyReport>>>(
                    ctx => ctx.Headers.Set(DailyReportConsumer.EventTimeUtcHeader, DateTimeOffset.UtcNow.ToString(CultureInfo.InvariantCulture)));

                var message = _bus.ScheduleRecurringMessage(
                    new Uri(_serviceConfig.RabbitMq.GetQueueUrl(WellKnownQueues.Notifications)),
                    schedule,
                    scheduleMessage,
                    schedulePipe).Result;
                
                _bus.ConnectPublishObserver(new Test());
                _bus.ConnectSendObserver(new Test());
                return true;
            }
            catch (Exception e)
            {
                LogFatal("An error occurred while starting Quartz scheduler", e);
                _scheduler.Shutdown();
                return false;
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

        private class Test : IPublishObserver, ISendObserver
        {
            public Task PrePublish<T>(PublishContext<T> context) where T : class
            {
                return Task.FromResult(true);
            }

            public Task PostPublish<T>(PublishContext<T> context) where T : class
            {
                return Task.FromResult(true);
            }

            public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
            {
                return Task.FromResult(true);
            }

            public Task PreSend<T>(SendContext<T> context) where T : class
            {
                var scheduleMessage = context.Message as ScheduledMessageJob;
                if (scheduleMessage == null)
                    return Task.FromResult(true);

                scheduleMessage.HeadersAsJson = JsonConvert.SerializeObject(new Dictionary<string, string> {{DailyReportConsumer.EventTimeUtcHeader, ""}});
                // context.Headers.Set(DailyReportConsumer.EventTimeUtcHeader, DateTimeOffset.UtcNow.ToString(CultureInfo.InvariantCulture));
                return Task.FromResult(true);
            }

            public Task PostSend<T>(SendContext<T> context) where T : class
            {
                return Task.FromResult(true);
            }

            public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
            {
                return Task.FromResult(true);
            }
        }
    }
}