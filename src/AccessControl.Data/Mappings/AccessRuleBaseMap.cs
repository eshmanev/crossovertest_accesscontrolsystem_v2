using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class AccessRuleBaseMap : ClassMap<AccessRuleBase>
    {
        public AccessRuleBaseMap()
        {
            Table("AccessRule");
            Id(x => x.Id).GeneratedBy.HiLo("AccessRule");
            References(x => x.AccessPoint).Column("AccessPointId").ForeignKey("FK_AccessRule_AccessPoint");
            References(x => x.AccessRights).Column("AccessRightsId").ForeignKey("FK_AccessRule_AccessRights");
            DiscriminateSubClassesOnColumn("RuleType");
            
        }
    }
}