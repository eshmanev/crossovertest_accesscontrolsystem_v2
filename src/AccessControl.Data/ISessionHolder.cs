using System;
using NHibernate;

namespace AccessControl.Data
{
    /// <summary>
    ///     Holds the NHibernate session.
    /// </summary>
    public interface ISessionHolder : IDisposable
    {
        /// <summary>
        ///     Commits the transaction if it was started.
        /// </summary>
        void Commit();

        /// <summary>
        ///     Demands the transaction.
        /// </summary>
        void DemandTransaction();

        /// <summary>
        ///     Gets the current session.
        /// </summary>
        /// <returns>The session</returns>
        ISession GetSession();

        /// <summary>
        ///     Rollbacks the transaction if it was started.
        /// </summary>
        void Rollback();
    }
}