using System;
using System.Collections.Generic;
using AccessControl.Data.Entities;
using NHibernate;

namespace AccessControl.Data.Session
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        private readonly ISessionFactoryHolder _sessionFactoryHolder;
        private ISession _session;
        private ITransaction _transaction;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseContext" /> class.
        /// </summary>
        /// <param name="sessionFactoryHolder">The session factory holder.</param>
        public DatabaseContext(ISessionFactoryHolder sessionFactoryHolder)
        {
            _sessionFactoryHolder = sessionFactoryHolder;
        }

        private bool IsInTransaction => _transaction != null && _transaction.IsActive;

        /// <summary>
        ///     Gets or sets the access points.
        /// </summary>
        /// <value>
        ///     The access points.
        /// </value>
        public IRepository<AccessPoint> AccessPoints => GetRepository<AccessPoint>();

        /// <summary>
        ///     Gets or sets the access rights.
        /// </summary>
        /// <value>
        ///     The access rights.
        /// </value>
        public IRepository<AccessRightsBase> AccessRights => GetRepository<AccessRightsBase>();

        /// <summary>
        ///     Gets or sets the delegated rights.
        /// </summary>
        /// <value>
        ///     The delegated rights.
        /// </value>
        public IRepository<DelegatedRights> DelegatedRights => GetRepository<DelegatedRights>();

        /// <summary>
        ///     Gets or sets the logs.
        /// </summary>
        /// <value>
        ///     The logs.
        /// </value>
        public IRepository<LogEntry> Logs => GetRepository<LogEntry>();

        /// <summary>
        ///     Gets or sets the users.
        /// </summary>
        /// <value>
        ///     The users.
        /// </value>
        public IRepository<User> Users => GetRepository<User>();

        /// <summary>
        ///     Demands the transaction.
        /// </summary>
        public void DemandTransaction()
        {
            if (_transaction == null)
            {
                _transaction = GetSession().BeginTransaction();
            }
        }

        /// <summary>
        ///     Gets the session.
        /// </summary>
        /// <returns></returns>
        public ISession GetSession()
        {
            return _session ?? (_session = _sessionFactoryHolder.SessionFactory.OpenSession());
        }

        /// <summary>
        ///     Commits this instance.
        /// </summary>
        public void Commit()
        {
            if (!IsInTransaction)
            {
                return;
            }

            _session.Transaction.Commit();
        }

        /// <summary>
        ///     Rollbacks this instance.
        /// </summary>
        public void Rollback()
        {
            if (!IsInTransaction)
            {
                return;
            }

            _session.Transaction.Rollback();
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            DisposeTransaction();
            DisposeSession();
        }

        private void DisposeSession()
        {
            _session?.Dispose();
        }

        private void DisposeTransaction()
        {
            if (!IsInTransaction)
            {
                return;
            }

            _session.Transaction.Dispose();
        }

        private IRepository<T> GetRepository<T>()
            where T : class
        {
            var type = typeof(T);
            object repository;
            if (!_repositories.TryGetValue(type, out repository))
            {
                repository = new Repository<T>(this);
                _repositories.Add(type, repository);
            }
            return (IRepository<T>) repository;
        }
    }
}