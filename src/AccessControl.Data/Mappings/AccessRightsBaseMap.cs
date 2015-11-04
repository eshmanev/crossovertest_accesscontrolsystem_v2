using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class AccessRightsBaseMap : ClassMap<AccessRightsBase>
    {
        public AccessRightsBaseMap()
        {
            Table("AccessRights");
            Id(x => x.Id).GeneratedBy.HiLo("AccessRights");
            HasMany(x => x.AccessRules).Inverse().KeyColumn("AccessRightsId").Cascade.AllDeleteOrphan();
            DiscriminateSubClassesOnColumn("AccessRightsType");
        }
    }
}