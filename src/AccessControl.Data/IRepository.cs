using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AccessControl.Data
{
    /// <summary>
    /// Represents a generic repository.
    /// </summary>
    /// <typeparam name="T">The type of entities in repository.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Fetches all entities from the repository.
        /// </summary>
        /// <returns>A list of entities.</returns>
        IList<T> GetAll();

        /// <summary>
        /// Returns an entity which has the given identifier.
        /// </summary>
        /// <param name="id">The entity's identifier.</param>
        /// <returns>An entity or null.</returns>
        T GetById(object id);

        /// <summary>
        /// Fetches all entities which match the given predicate.
        /// </summary>
        /// <param name="predicate">A function which specifies a condition.</param>
        /// <returns>A list of entities.</returns>
        IList<T> Filter(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Returns a number of entities in repository.
        /// </summary>
        /// <returns>A number of entities.</returns>
        int GetCount();

        /// <summary>
        /// Returns a number of entities which matche the given predicate.
        /// </summary>
        /// <param name="predicate">A function which specifies a condition.</param>
        /// <returns>A number of entities.</returns>
        int GetCount(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Inserts the specified entity into repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Insert(T entity);

        /// <summary>
        /// Updates the specified entity in repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);

        /// <summary>
        /// Deletes the specified entity from repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);
    }
}