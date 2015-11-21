using System;
using NHibernate;

namespace AccessControl.Data
{
    /// <summary>
    ///     Holds the NHibernate session.
    /// </summary>
    public interface ISessionScope : IDisposable
    {
        /// <summary>
        ///     Gets the current session.
        /// </summary>
        /// <returns>The session</returns>
        ISession GetSession();

        /// <summary>
        ///     Gets the repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entities in the repository.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T> GetRepository<T>() where T : class;
         
        /// <summary>
        ///     Demands the transaction.
        /// </summary>
        void DemandTransaction();

        /// <summary>
        ///     Commits the transaction if it was started.
        /// </summary>
        void Commit();

        /// <summary>
        ///     Rollbacks the transaction if it was started.
        /// </summary>
        void Rollback();
    }
}