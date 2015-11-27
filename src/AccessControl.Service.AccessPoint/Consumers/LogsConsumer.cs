using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Events;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Contracts.Impl.Dto;
using AccessControl.Data;
using log4net;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Consumers
{
    public class LoggingConsumer : IConsumer<IAccessAttempted>, IConsumer<IListLogs>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LoggingConsumer));
        private readonly IRepository<Data.Entities.AccessPoint> _accessPointRepository;
        private readonly IRequestClient<IListUsers, IListUsersResult> _listUsersRequest;
        private readonly IRepository<Data.Entities.LogEntry> _logRepository;
        private readonly IRepository<Data.Entities.User> _userRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoggingConsumer" /> class.
        /// </summary>
        /// <param name="logRepository">The log repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="accessPointRepository">The access point repository.</param>
        /// <param name="listUsersRequest">The list users request.</param>
        public LoggingConsumer(IRepository<Data.Entities.LogEntry> logRepository,
                               IRepository<Data.Entities.User> userRepository,
                               IRepository<Data.Entities.AccessPoint> accessPointRepository,
                               IRequestClient<IListUsers, IListUsersResult> listUsersRequest)
        {
            Contract.Requires(logRepository != null);
            Contract.Requires(userRepository != null);
            Contract.Requires(listUsersRequest != null);
            Contract.Requires(accessPointRepository != null);

            _logRepository = logRepository;
            _userRepository = userRepository;
            _accessPointRepository = accessPointRepository;
            _listUsersRequest = listUsersRequest;
        }

        /// <summary>
        ///     Logs the access attempt.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task Consume(ConsumeContext<IAccessAttempted> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.ClientService))
            {
                Log.Warn("Invalid credential.");
                return Task.FromResult(false);
            }

            var accessPoint = _accessPointRepository.GetById(context.Message.AccessPointId);
            if (accessPoint == null)
            {
                Log.Warn("Invalid access point attempted. AccessPointId: " + context.Message.AccessPointId);
                return Task.FromResult(false);
            }

            var user = _userRepository.Filter(x => x.BiometricHash == context.Message.BiometricHash).SingleOrDefault();
            var entry = new Data.Entities.LogEntry
            {
                AccessPoint = accessPoint,
                AttemptedHash = context.Message.BiometricHash,
                UserName = user?.UserName,
                CreatedUtc = context.Message.CreatedUtc,
                Failed = context.Message.Failed
            };
            _logRepository.Insert(entry);
            return Task.FromResult(true);
        }

        /// <summary>
        ///     Lists log entries.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IListLogs> context)
        {
            var logs = new List<ILogEntry>();
            if (Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                var usersResult = await _listUsersRequest.Request(ListCommand.WithoutParameters);
                var users = usersResult.Users.ToDictionary(x => x.UserName);
                var entities = _logRepository.Filter(
                    x => x.CreatedUtc >= context.Message.FromDateUtc &&
                         x.CreatedUtc <= context.Message.ToDateUtc);

                foreach (var entity in entities)
                {
                    IUser user;
                    users.TryGetValue(entity.UserName, out user);

                    var accessPoint = new Contracts.Helpers.AccessPoint(
                        entity.AccessPoint.AccessPointId,
                        entity.AccessPoint.Site,
                        entity.AccessPoint.Department,
                        entity.AccessPoint.Name)
                    {
                        Description = entity.AccessPoint.Description
                    };
                    logs.Add(new LogEntry(entity.CreatedUtc, accessPoint, entity.AttemptedHash, user, entity.Failed));
                }
            }
            await context.RespondAsync(ListCommand.LogsResult(logs.ToArray()));
        }
    }
}