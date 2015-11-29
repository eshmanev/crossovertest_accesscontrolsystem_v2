using System;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Events;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Service.Notifications.Messages;
using AccessControl.Service.Notifications.Services;
using log4net;
using MassTransit;

namespace AccessControl.Service.Notifications.Consumers
{
    internal class NotificationConsumer : IConsumer<IManagementRightsGranted>, IConsumer<IManagementRightsRevoked>, IConsumer<IAccessAttempted>
    {
        private readonly IRequestClient<IFindAccessPointById, IFindAccessPointByIdResult> _findAccessPointRequest;
        private readonly IRequestClient<IFindUserByBiometrics, IFindUserByBiometricsResult> _findUserByBiometricsRequest;
        private readonly IRequestClient<IFindUserByName, IFindUserByNameResult> _findUserByNameRequest;
        private readonly INotificationService _notificationService;
        private static readonly ILog Log = LogManager.GetLogger(typeof(NotificationConsumer));

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotificationConsumer" /> class.
        /// </summary>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="findUserByNameRequest">The find user by name request.</param>
        /// <param name="findUserByBiometricsRequest">The find user by biometrics request.</param>
        /// <param name="findAccessPointRequest">The find access point request.</param>
        public NotificationConsumer(INotificationService notificationService,
                                    IRequestClient<IFindUserByName, IFindUserByNameResult> findUserByNameRequest,
                                    IRequestClient<IFindUserByBiometrics, IFindUserByBiometricsResult> findUserByBiometricsRequest,
                                    IRequestClient<IFindAccessPointById, IFindAccessPointByIdResult> findAccessPointRequest)
        {
            Contract.Requires(notificationService != null);
            Contract.Requires(findUserByNameRequest != null);
            Contract.Requires(findUserByBiometricsRequest != null);
            Contract.Requires(findAccessPointRequest != null);

            _notificationService = notificationService;
            _findUserByNameRequest = findUserByNameRequest;
            _findUserByBiometricsRequest = findUserByBiometricsRequest;
            _findAccessPointRequest = findAccessPointRequest;
        }

        /// <summary>
        ///     Notifies a manager when an access is failed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IAccessAttempted> context)
        {
            if (!context.Message.Failed)
            {
                return;
            }

            var findUserResult = await _findUserByBiometricsRequest.Request(new FindUserByBiometrics(context.Message.BiometricHash));
            if (findUserResult.User == null)
            {
                return;
            }

            var user = findUserResult.User;
            var findManagerTask = _findUserByNameRequest.Request(new FindUserByName(user.ManagerName));
            var findAccessPointTaks = _findAccessPointRequest.Request(new FindAccessPointById(context.Message.AccessPointId));

            await Task.WhenAll(findManagerTask, findAccessPointTaks);

            var findManagerResult = findManagerTask.Result;
            if (findManagerResult.User == null)
            {
                return;
            }

            var accessPoint = findAccessPointTaks.Result.AccessPoint;

            if (!string.IsNullOrWhiteSpace(findManagerResult.User.Email))
            {
                var body = new StringBuilder();
                body.AppendLine($"Dear {findManagerResult.User.DisplayName},");
                body.AppendLine($"You have received this email because an access violation occurred.");

                if (accessPoint == null)
                {
                    body.AppendLine($"Unknown access point, ID={context.Message.AccessPointId}");
                }
                else
                {
                    body.AppendLine("Site: " + accessPoint.Site);
                    body.AppendLine("Department: " + accessPoint.Department);
                    body.AppendLine("Access Point: " + accessPoint.Name);
                }
                body.AppendLine("User: " + (user.DisplayName ?? user.UserName));
                body.AppendLine("Best regards.");
                SendEmail(context, findManagerResult.User.Email, "Access violation occurred", body.ToString());
            }

            if (!string.IsNullOrWhiteSpace(findManagerResult.User.PhoneNumber))
            {
                var body = new StringBuilder();
                body.AppendLine("Access violation occurred.");
                if (accessPoint == null)
                {
                    body.AppendLine($"Unknown access point, ID={context.Message.AccessPointId}");
                }
                else
                {
                    body.AppendLine($"Access Point {accessPoint.Name}");
                }
                body.AppendLine("User: " + (user.DisplayName ?? user.UserName));

                SendSms(context, findManagerResult.User.PhoneNumber, body.ToString());
            }
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

            var body = new StringBuilder();
            body.AppendLine($"Dear {findUserResult.User.DisplayName},");
            body.AppendLine("You are granted management and monitoring rights of the Access Control System");
            body.AppendLine("Best regards.");
            SendEmail(context, findUserResult.User.Email, "Management rights granted", body.ToString());
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

            var body = new StringBuilder();
            body.AppendLine($"Dear {findUserResult.User.DisplayName},");
            body.AppendLine("You are revoked management and monitoring rights of the Access Control System");
            body.AppendLine("Best regards.");
            SendEmail(context, findUserResult.User.Email, "Management rights revoked", body.ToString());
        }

        private void SendEmail(ConsumeContext context, string recipient, string subject, string body)
        {
            try
            {
                _notificationService.SendEmail(recipient, subject, body);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while sending an email", ex);
                context.ScheduleMessage(
                    DateTime.Now.AddTicks(RedeliveryConsumer.RedeliveryInterval.Ticks),
                    new RedeliverEmailMessage
                    {
                        EmailAddress = recipient,
                        Subject = subject,
                        Body = body
                    });
            }
        }

        private void SendSms(ConsumeContext context, string phone, string text)
        {
            try
            {
                _notificationService.SendSms(phone, text);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while sending SMS message", ex);
                context.ScheduleMessage(
                    DateTime.Now.AddTicks(RedeliveryConsumer.RedeliveryInterval.Ticks),
                    new RedeliverSmsMessage
                    {
                        PhoneNumber = phone,
                        Text = text
                    });
            }
        }
    }
}