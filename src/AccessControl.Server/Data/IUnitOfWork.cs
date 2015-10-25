using System;

namespace AccessControl.Server.Data
{
    /// <summary>
    /// Represents a Unit of Work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits the changes made to repositories.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks the changes made to repositories.
        /// </summary>
        void Rollback();
    }
}