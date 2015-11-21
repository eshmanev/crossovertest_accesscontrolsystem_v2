using NHibernate;

namespace AccessControl.Data.Session
{
    internal class SessionScope : ISessionScope
    {
        private readonly ISessionFactoryHolder _sessionFactoryHolder;
        private ISession _session;
        private ITransaction _transaction;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SessionScope" /> class.
        /// </summary>
        /// <param name="sessionFactoryHolder">The session factory holder.</param>
        public SessionScope(ISessionFactoryHolder sessionFactoryHolder)
        {
            _sessionFactoryHolder = sessionFactoryHolder;
        }

        private bool IsInTransaction => _transaction != null && _transaction.IsActive;

        /// <summary>
        ///     Gets the repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entities in the repository.</typeparam>
        /// <returns>The repository.</returns>
        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(this);
        }

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
    }
}