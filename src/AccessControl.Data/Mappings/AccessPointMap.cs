using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class AccessPointMap : ClassMap<AccessPoint>
    {
        public AccessPointMap()
        {
            Id(x => x.AccessPointId).GeneratedBy.Assigned();
            Map(x => x.Name).Unique();
            Map(x => x.Description);
            Map(x => x.Site);
            Map(x => x.Department);
        }    
    }
}