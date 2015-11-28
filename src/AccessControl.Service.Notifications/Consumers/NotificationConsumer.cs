using System;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Events;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Service.Notifications.Services;
using log4net;
using MassTransit;

namespace AccessControl.Service.Notifications.Consumers
{
    internal class NotificationConsumer : IConsumer<IAccessAttempted>, IConsumer<IManagementRightsGranted>, IConsumer<IManagementRightsRevoked>
    {
        private readonly IRequestClient<IFindUserByName, IFindUserByNameResult> _findUserByNameRequest;
        private readonly INotificationService _notificationService;
        private static readonly ILog Log = LogManager.GetLogger(typeof(NotificationConsumer));

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotificationConsumer" /> class.
        /// </summary>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="findUserByNameRequest">The find user by name request.</param>
        public NotificationConsumer(INotificationService notificationService,
                                    IRequestClient<IFindUserByName, IFindUserByNameResult> findUserByNameRequest)
        {
            Contract.Requires(notificationService != null);
            Contract.Requires(findUserByNameRequest != null);
            _notificationService = notificationService;
            _findUserByNameRequest = findUserByNameRequest;
        }

        public Task Consume(ConsumeContext<IAccessAttempted> context)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///     Sends notification when a manager grants his management rights.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IManagementRightsGranted> context)
        {
            var findUserResult = await _findUserByNameRequest.Request(new FindUserByName(context.Message.Grantee));
            if (findUserResult.User == null || string.IsNullOrWhiteSpace(findUserResult.User.Email))
            {
                return;
            }

            try
            {
                var user = findUserResult.User;
                var body = new StringBuilder();
                body.AppendLine($"Dear {user.DisplayName},");
                body.AppendLine("You are granted management and monitoring rights of the Access Control System");
                body.AppendLine("Best regards.");
                _notificationService.SendEmail(user.Email, "Management rights granted", body.ToString());
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while sending an email", ex);
                await context.Redeliver(TimeSpan.FromSeconds(3));
            }
        }

        /// <summary>
        ///     Sends notification when a manager revokes his management rights.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IManagementRightsRevoked> context)
        {
            var findUserResult = await _findUserByNameRequest.Request(new FindUserByName(context.Message.Grantee));
            if (findUserResult.User == null || string.IsNullOrWhiteSpace(findUserResult.User.Email))
            {
                return;
            }

            try
            {
                var user = findUserResult.User;
                var body = new StringBuilder();
                body.AppendLine($"Dear {user.DisplayName},");
                body.AppendLine("You are revoked management and monitoring rights of the Access Control System");
                body.AppendLine("Best regards.");
                _notificationService.SendEmail(user.Email, "Management rights revoked", body.ToString());
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while sending an email", ex);
                await context.Redeliver(TimeSpan.FromSeconds(3));
            }
        }
    }
}