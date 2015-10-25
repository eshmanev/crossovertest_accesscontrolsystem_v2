﻿using AccessControl.Data.Entities;
using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    public class UserHashMapMap : ClassMap<UserHashMap>
    {
        public UserHashMapMap()
        {
            Id(x => x.Id).GeneratedBy.HiLo<UserHashMap>();
            Map(x => x.UserName).Unique();
            Map(x => x.UserHash).Unique();
        }
    }
}