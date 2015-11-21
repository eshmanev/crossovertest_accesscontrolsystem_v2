using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.Data.Configuration;
using FluentNHibernate.Cfg.Db;

namespace AccessControl.Data.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IDataConfiguration" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IDataConfiguration))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IDataConfigurationContract : IDataConfiguration
    {
        public bool RecreateDatabaseSchema => false;

        public string ConnectionString
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public IPersistenceConfigurer PersistenceConfigurer
        {
            get
            {
                Contract.Ensures(Contract.Result<IPersistenceConfigurer>() != null);
                return null;
            }
        }

        public IEnumerable<Type> DialectScopes
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Type>>() != null);
                return null;
            }
        }
    }
}