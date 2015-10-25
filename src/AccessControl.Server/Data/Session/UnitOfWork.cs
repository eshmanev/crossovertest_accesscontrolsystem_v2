using System.Data;
using System.Diagnostics.Contracts;
using NHibernate;

namespace AccessControl.Server.Data.Session
{
    /// <summary>
    ///     Represents a Unit of Work.
    /// </summary>
    internal class UnitOfWork : IUnitOfWork, ISessionLocator
    {
        private readonly ISessionFactoryHolder _sessionFactoryHolder;
        private ISession _session;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        public UnitOfWork(ISessionFactoryHolder sessionFactoryHolder)
        {
            Contract.Requires(sessionFactoryHolder != null);
            _sessionFactoryHolder = sessionFactoryHolder;
            IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        ///     Gets or sets the transaction isolation level.
        /// </summary>
        public IsolationLevel IsolationLevel { get; set; }

        /// <summary>
        ///     Gets the current session.
        /// </summary>
        public ISession GetSession()
        {
            EnsureSession();
            return _session;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DisposeSession();
        }

        /// <summary>
        ///     Commits the changes made to repositories.
        /// </summary>
        public void Commit()
        {
            if (_session != null && !_session.Transaction.WasRolledBack && _session.Transaction.IsActive)
            {
                _session.Transaction.Commit();
            }
        }

        /// <summary>
        ///     Rollbacks the changes made to repositories.
        /// </summary>
        public void Rollback()
        {
            if (_session != null && !_session.Transaction.WasRolledBack && _session.Transaction.IsActive)
            {
                _session.Transaction.Rollback();
            }
        }

        private void DisposeSession()
        {
            if (_session == null)
            {
                return;
            }
            try
            {
                if (!_session.Transaction.WasRolledBack && _session.Transaction.IsActive)
                {
                    _session.Transaction.Commit();
                }
            }
            finally
            {
                _session.Close();
                _session.Dispose();
                _session = null;
            }
        }

        private void EnsureSession()
        {
            if (_session != null)
            {
                return;
            }

            _session = _sessionFactoryHolder.SessionFactory.OpenSession();
            _session.BeginTransaction(IsolationLevel);
        }
    }
}