using System;
using System.Collections.Generic;
using System.Configuration;
using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;

namespace AccessControl.Data.Configuration
{
    public class DataConfiguration : ConfigurationSection, IDataConfiguration
    {
        [ConfigurationProperty("recreateDatabaseSchema", DefaultValue = false)]
        public bool RecreateDatabaseSchema => (bool) base["recreateDatabaseSchema"];

        [ConfigurationProperty("dialect", DefaultValue = "SqlCompact")]
        public SqlDialect Dialect => (SqlDialect) base["dialect"];

        [ConfigurationProperty("connectionString", DefaultValue = "database.sdf")]
        public string ConnectionString => (string) base["connectionString"];

        IPersistenceConfigurer IDataConfiguration.PersistenceConfigurer
        {
            get
            {
                switch (Dialect)
                {
                    case SqlDialect.SqlCompact:
                        return MsSqlCeConfiguration.MsSqlCe40.ConnectionString(ConnectionString);

                    case SqlDialect.SqlServer:
                    case SqlDialect.SqlAzure:
                        return MsSqlConfiguration.MsSql2012.ConnectionString(ConnectionString);

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        IEnumerable<Type> IDataConfiguration.DialectScopes
        {
            get
            {
                switch (Dialect)
                {
                    case SqlDialect.SqlCompact:
                        return new[]
                        {
                            typeof(MsSqlCeDialect),
                        };

                    case SqlDialect.SqlServer:
                        return new[]
                        {
                            typeof(MsSql2000Dialect),
                            typeof(MsSql2005Dialect),
                            typeof(MsSql2008Dialect),
                            typeof(MsSql2012Dialect),
                        };

                    case SqlDialect.SqlAzure:
                        return new[]
                        {
                            typeof(MsSqlAzure2008Dialect)
                        };

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}