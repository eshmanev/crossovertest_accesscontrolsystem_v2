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
        private readonly IDatabaseContext _databaseContext;
        private readonly IRequestClient<IListUsers, IListUsersResult> _listUsersRequest;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoggingConsumer" /> class.
        /// </summary>
        /// <param name="databaseContext">The database context.</param>
        /// <param name="listUsersRequest">The list users request.</param>
        public LoggingConsumer(IDatabaseContext databaseContext,
                               IRequestClient<IListUsers, IListUsersResult> listUsersRequest)
        {
            Contract.Requires(databaseContext != null);
            Contract.Requires(listUsersRequest != null);

            _databaseContext = databaseContext;
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

            var accessPoint = _databaseContext.AccessPoints.GetById(context.Message.AccessPointId);
            if (accessPoint == null)
            {
                Log.Warn("Invalid access point attempted. AccessPointId: " + context.Message.AccessPointId);
                return Task.FromResult(false);
            }

            var user = _databaseContext.Users.Filter(x => x.BiometricHash == context.Message.BiometricHash).SingleOrDefault();
            var entry = new Data.Entities.LogEntry
            {
                AccessPoint = accessPoint,
                AttemptedHash = context.Message.BiometricHash,
                UserName = user?.UserName,
                CreatedUtc = context.Message.CreatedUtc,
                Failed = context.Message.Failed
            };
            _databaseContext.Logs.Insert(entry);
            _databaseContext.Commit();
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
                var entities = _databaseContext.Logs.Filter(
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