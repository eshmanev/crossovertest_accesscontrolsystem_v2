using NHibernate;

namespace AccessControl.Data.Session
{
    public class SessionHolder : ISessionHolder
    {
        private readonly ISessionFactoryHolder _sessionFactoryHolder;
        private ISession _session;
        private ITransaction _transaction;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SessionHolder" /> class.
        /// </summary>
        /// <param name="sessionFactoryHolder">The session factory holder.</param>
        public SessionHolder(ISessionFactoryHolder sessionFactoryHolder)
        {
            _sessionFactoryHolder = sessionFactoryHolder;
        }

        private bool IsInTransaction => _transaction != null && _transaction.IsActive;

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