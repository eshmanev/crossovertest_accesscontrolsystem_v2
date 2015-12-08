using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class ScheduleEntryMap : ClassMap<SchedulerEntry>
    {
        public ScheduleEntryMap()
        {
            Id(x => x.Id).GeneratedBy.HiLo<SchedulerEntry>();
            Map(x => x.FromTime);
            Map(x => x.ToTime);
            Map(x => x.Day);
            References(x => x.Rule).Column("AccessRuleId").ForeignKey("FK_ScheduledEntry_AccessRule");
        }
    }
}