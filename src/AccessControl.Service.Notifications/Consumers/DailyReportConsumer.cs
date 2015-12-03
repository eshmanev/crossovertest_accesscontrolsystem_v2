using System;
using System.Threading.Tasks;
using AccessControl.Service.Notifications.Messages;
using MassTransit;

namespace AccessControl.Service.Notifications.Consumers
{
    internal class DailyReportConsumer : IConsumer<GenerateDailyReport>
    {
        /// <summary>
        ///     Represents a header key with the message fired time, in UTC format.
        /// </summary>
        public const string EventTimeUtcHeader = "EventTimeUtc";

        public Task Consume(ConsumeContext<GenerateDailyReport> context)
        {
            object value;
            if (!context.Headers.TryGetHeader(EventTimeUtcHeader, out value))
            {
                throw new InvalidOperationException($"{EventTimeUtcHeader} header expected");
            }
            return Task.FromResult(true);
        }
    }
}