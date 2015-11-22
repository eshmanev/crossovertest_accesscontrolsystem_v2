using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class ScheduledAccessRuleMap : SubclassMap<ScheduledAccessRule>
    {
        public ScheduledAccessRuleMap()
        {
            DiscriminatorValue(1);
            Map(x => x.FromTimeUtc);
            Map(x => x.ToTimeUtc);
        }
    }
}