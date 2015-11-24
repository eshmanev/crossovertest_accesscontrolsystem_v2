using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class DelegatedRightsMap : ClassMap<DelegatedRights>
    {
        public DelegatedRightsMap()
        {
            Id(x => x.Id).GeneratedBy.HiLo<DelegatedRights>();
            Map(x => x.Grantor);
            Map(x => x.Grantee);
        }
    }
}