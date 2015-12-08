using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Data;
using AccessControl.Data.Entities;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Services
{
    internal class ScheduledUserAccessStrategy : UserAccessStrategyBase<ScheduledAccessRule>
    {
        private readonly IScheduleUserAccess _message;

        public ScheduledUserAccessStrategy(IDatabaseContext databaseContext, IRequestClient<IFindUserByName, IFindUserByNameResult> findUserRequest, IScheduleUserAccess message)
            : base(databaseContext, findUserRequest, message.UserName)
        {
            Contract.Requires(message != null);
            _message = message;
        }

        public override AccessRuleBase CreateAccessRule()
        {
            var rule = (ScheduledAccessRule)base.CreateAccessRule();
            rule.TimeZone = _message.Schedule.TimeZone;
            foreach (var item in _message.Schedule.DailyTimeRange)
            {
                var entry = new SchedulerEntry
                {
                    Day = item.Key,
                    FromTime = item.Value.FromTime,
                    ToTime = item.Value.ToTime,
                };
                rule.AddEntry(entry);
            }
            return rule;
        }

        public override Task OnAccessGranted(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            throw new System.NotImplementedException();
        }

        public override Task OnAccessDenied(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            throw new System.NotImplementedException();
        }
    }
}