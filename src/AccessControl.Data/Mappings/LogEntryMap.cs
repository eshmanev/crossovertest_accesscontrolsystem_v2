using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class LogEntryMap : ClassMap<LogEntry>
    {
        public LogEntryMap()
        {
            Id(x => x.Id).GeneratedBy.HiLo<LogEntry>();
            Map(x => x.CreatedUtc).Not.Nullable();
            Map(x => x.AccessPointId).Not.Nullable();
            Map(x => x.AttemptedHash).Not.Nullable();
            Map(x => x.UserName);
        }
    }
}