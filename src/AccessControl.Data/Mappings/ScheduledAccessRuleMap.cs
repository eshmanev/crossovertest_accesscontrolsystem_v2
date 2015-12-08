using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class ScheduledAccessRuleMap : SubclassMap<ScheduledAccessRule>
    {
        public ScheduledAccessRuleMap()
        {
            DiscriminatorValue(1);
            Map(x => x.TimeZone);
            HasMany(x => x.Entries).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}