using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class UserAccessRightsMap : SubclassMap<UserAccessRights>
    {
        public UserAccessRightsMap()
        {
            DiscriminatorValue(0);
            Map(x => x.UserName);
        }
    }
}