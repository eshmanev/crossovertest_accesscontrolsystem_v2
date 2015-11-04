using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class PermanentAccessRuleMap : SubclassMap<PermanentAccessRule>
    {
        public PermanentAccessRuleMap()
        {
            DiscriminatorValue(0);
        }
    }
}