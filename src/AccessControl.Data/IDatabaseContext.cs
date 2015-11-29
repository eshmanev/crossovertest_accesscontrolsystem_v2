using System;
using AccessControl.Data.Entities;
using NHibernate;

namespace AccessControl.Data
{
    /// <summary>
    ///     Holds the NHibernate session.
    /// </summary>
    public interface IDatabaseContext : IDisposable
    {
        /// <summary>
        /// Gets a repository of the access points.
        /// </summary>
        /// <value>
        /// The access points.
        /// </value>
        IRepository<AccessPoint> AccessPoints { get; }

        /// <summary>
        /// Gets a repository of the access rights.
        /// </summary>
        /// <value>
        /// The access rights.
        /// </value>
        IRepository<AccessRightsBase> AccessRights { get; }

        /// <summary>
        /// Gets a repository of the delegated rights.
        /// </summary>
        /// <value>
        /// The delegated rights.
        /// </value>
        IRepository<DelegatedRights> DelegatedRights { get; }

        /// <summary>
        /// Gets a repository of the logs.
        /// </summary>
        /// <value>
        /// The logs.
        /// </value>
        IRepository<LogEntry> Logs { get; }

        /// <summary>
        /// Gets a repository of the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        IRepository<User> Users { get; }

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
        ///     Commits the transaction if it was started.
        /// </summary>
        void Commit();

        /// <summary>
        ///     Rollbacks the transaction if it was started.
        /// </summary>
        void Rollback();
    }
}