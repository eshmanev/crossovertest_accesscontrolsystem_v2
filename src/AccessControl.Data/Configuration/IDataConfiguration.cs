using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.Data.CodeContracts;
using FluentNHibernate.Cfg.Db;

namespace AccessControl.Data.Configuration
{
    /// <summary>
    /// Represents a data configuration.
    /// </summary>
    [ContractClass(typeof(IDataConfigurationContract))]
    public interface IDataConfiguration
    {
        /// <summary>
        /// Gets a value indicating whether database schema should be recreated at startup.
        /// </summary>
        /// <value>
        /// <c>true</c> if database schema should be recreated; otherwise, <c>false</c>.
        /// </value>
        bool RecreateDatabaseSchema { get; }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the persistence configurer.
        /// </summary>
        /// <value>
        /// The persistence configurer.
        /// </value>
        IPersistenceConfigurer PersistenceConfigurer { get; }

        /// <summary>
        /// Gets the dialect scopes.
        /// </summary>
        /// <value>
        /// The dialect scopes.
        /// </value>
        IEnumerable<Type> DialectScopes { get; } 
    }
}