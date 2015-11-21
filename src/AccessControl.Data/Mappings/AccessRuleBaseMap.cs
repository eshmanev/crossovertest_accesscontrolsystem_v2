using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class AccessRuleBaseMap : ClassMap<AccessRuleBase>
    {
        public AccessRuleBaseMap()
        {
            Table("AccessRule");
            Id(x => x.Id).GeneratedBy.Guid();
            References(x => x.AccessPoint).Column("AccessPointId").Not.Nullable();
            References(x => x.AccessRights).Column("AccessRightsId").Not.Nullable();
            DiscriminateSubClassesOnColumn("RuleType");
        }
    }
}