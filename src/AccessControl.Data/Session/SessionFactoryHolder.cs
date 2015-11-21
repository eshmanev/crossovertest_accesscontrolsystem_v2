using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using AccessControl.Data.Configuration;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;

namespace AccessControl.Data.Session
{
    /// <summary>
    ///     Represents an implementation of session factory holder.
    /// </summary>
    public class SessionFactoryHolder : ISessionFactoryHolder, ISessionScopeFactory
    {
        private readonly IDataConfiguration _configuration;
        private readonly object _syncRoot = new object();
        private volatile ISessionFactory _sessionFactory;

        public SessionFactoryHolder(IDataConfiguration configuration)
        {
            Contract.Requires(configuration != null);
            _configuration = configuration;
        }

        /// <summary>
        ///     Gets the session factory.
        /// </summary>
        public ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory != null)
                {
                    return _sessionFactory;
                }

                lock (_syncRoot)
                {
                    if (_sessionFactory != null)
                    {
                        return _sessionFactory;
                    }

                    _sessionFactory = CreateSessionFactory();
                }

                return _sessionFactory;
            }
        }

        public ISessionScope Create()
        {
            return new SessionScope(this);
        }

        //private void ConfigureHiLoTable(NHibernate.Cfg.Configuration configuration)
        //{
        //    var script = new StringBuilder();

        //    script.AppendLine("ALTER TABLE HiLo ADD [Entity] VARCHAR(128) NULL");
        //    script.AppendLine("GO");

        //    script.AppendLine("CREATE NONCLUSTERED INDEX IX_HiLo_Entity ON HiLo (Entity ASC);");
        //    script.AppendLine("GO");

        //    foreach (var tableName in configuration.ClassMappings.Select(m => m.Table.Name).Distinct())
        //    {
        //        script.AppendLine($"INSERT INTO [HiLo] (Entity, NextHi) VALUES ('{tableName}',1);");
        //    }

        //    configuration.AddAuxiliaryDatabaseObject(
        //        new SimpleAuxiliaryDatabaseObject(
        //            script.ToString(),
        //            null,
        //            new HashSet<string>(_configuration.DialectScopes.Select(x => x.FullName))));
        //}

        private ISessionFactory CreateSessionFactory()
        {
            var configuration = Fluently.Configure()
                                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SessionFactoryHolder>())
                                        .Database(_configuration.PersistenceConfigurer)
                                        // .ExposeConfiguration(ConfigureHiLoTable)
                                        .BuildConfiguration();

            if (_configuration.RecreateDatabaseSchema)
            {
                GenerateSchema(configuration);
            }
            else
            {
                EnsureSchemaExists(configuration);
            }

            return configuration.BuildSessionFactory();
        }

        private void EnsureSchemaExists(NHibernate.Cfg.Configuration configuration)
        {
            try
            {
                new SchemaValidator(configuration).Validate();
            }
            catch (HibernateException)
            {
                GenerateSchema(configuration);
            }
        }

        private void GenerateSchema(NHibernate.Cfg.Configuration configuration)
        {
            var schemaExport = new SchemaExport(configuration);
            schemaExport.Drop(false, true);
            schemaExport.Create(true, true);
        }
    }
}