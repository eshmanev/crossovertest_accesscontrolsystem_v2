using System.Collections.Generic;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Contracts.Impl.Dto;
using AccessControl.Data.Entities;
using Microsoft.Practices.ObjectBuilder2;

namespace AccessControl.Service.AccessPoint.Helpers
{
    /// <summary>
    ///     Represents a converter of access rights.
    /// </summary>
    internal class ConvertAccessRightsVisitor : IAccessRightsVisitor
    {
        public ConvertAccessRightsVisitor()
        {
            UserAccessRightsDto = new List<IUserAccessRights>();
            UserGroupAccessRightsDto = new List<IUserGroupAccessRights>();
        }

        public List<IUserAccessRights> UserAccessRightsDto { get; }

        public List<IUserGroupAccessRights> UserGroupAccessRightsDto { get; }

        public void Visit(UserAccessRights accessRights)
        {
            var rules = ConvertRules(accessRights);
            var dto = AccessRights.ForUser(accessRights.UserName, rules.Permanent.ToArray(), rules.Scheduled.ToArray());
            UserAccessRightsDto.Add(dto);
        }

        public void Visit(UserGroupAccessRights accessRights)
        {
            var rules = ConvertRules(accessRights);
            var dto = AccessRights.ForUserGroup(accessRights.UserGroupName, rules.Permanent.ToArray());
            UserGroupAccessRightsDto.Add(dto);
        }

        private ConvertAccessRuleVisitor ConvertRules(AccessRightsBase accessRights)
        {
            var visitor = new ConvertAccessRuleVisitor();
            accessRights.AccessRules.ForEach(x => x.Accept(visitor));
            return visitor;
        }

        private class ConvertAccessRuleVisitor : IAccessRuleVisitor
        {
            public ConvertAccessRuleVisitor()
            {
                Permanent = new List<IPermanentAccessRule>();
                Scheduled = new List<IScheduledAccessRule>();
            }

            public List<IPermanentAccessRule> Permanent { get; }

            public List<IScheduledAccessRule> Scheduled { get; }

            public void Visit(PermanentAccessRule rule)
            {
                var dto = AccessRule.Permanent(rule.AccessPoint.AccessPointId);
                Permanent.Add(dto);
            }

            public void Visit(ScheduledAccessRule rule)
            {
                var schedule = new Schedule(rule.TimeZone);
                rule.Entries.ForEach(x => schedule.DailyTimeRange.Add(x.Day, new TimeRange(x.FromTime, x.ToTime)));
                var dto = AccessRule.Scheduled(rule.AccessPoint.AccessPointId, schedule);
                Scheduled.Add(dto);
            }
        }
    }
}