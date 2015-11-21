using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.UserName).Unique();
            Map(x => x.BiometricHash).Unique();
        }
    }
}