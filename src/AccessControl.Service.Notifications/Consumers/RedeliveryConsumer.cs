using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Service.Notifications.Messages;
using AccessControl.Service.Notifications.Services;
using MassTransit;

namespace AccessControl.Service.Notifications.Consumers
{
    internal class RedeliveryConsumer : IConsumer<RedeliverEmailMessage>, IConsumer<RedeliverSmsMessage>
    {
        private readonly INotificationService _notificationService;
        public static readonly TimeSpan RedeliveryInterval = TimeSpan.FromMinutes(15);

        /// <summary>
        ///     Initializes a new instance of the <see cref="RedeliveryConsumer" /> class.
        /// </summary>
        /// <param name="notificationService">The notification service.</param>
        public RedeliveryConsumer(INotificationService notificationService)
        {
            Contract.Requires(notificationService != null);
            _notificationService = notificationService;
        }

        /// <summary>
        ///     Redeliveries failed email messages.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<RedeliverEmailMessage> context)
        {
            try
            {
                _notificationService.SendEmail(context.Message.EmailAddress, context.Message.Subject, context.Message.Body);
                return Task.FromResult(true);
            }
            catch
            {
                return context.Redeliver(RedeliveryInterval);
            }
        }

        /// <summary>
        ///     Redeliveries failed sms messages.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<RedeliverSmsMessage> context)
        {
            try
            {
                _notificationService.SendSms(context.Message.PhoneNumber, context.Message.Text);
                return Task.FromResult(true);
            }
            catch
            {
                return context.Redeliver(RedeliveryInterval);
            }
        }
    }
}