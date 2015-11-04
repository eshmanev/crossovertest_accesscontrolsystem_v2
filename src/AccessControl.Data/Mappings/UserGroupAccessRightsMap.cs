using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class UserGroupAccessRightsMap : SubclassMap<UserGroupAccessRights>
    {
        public UserGroupAccessRightsMap()
        {
            DiscriminatorValue(1);
            Map(x => x.UserGroupName);
        }
    }
}