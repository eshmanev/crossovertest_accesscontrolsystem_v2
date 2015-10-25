using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace AccessControl.Server.Data.Session
{
    /// <summary>
    ///     Represents a generic repository.
    /// </summary>
    /// <typeparam name="T">The type of entities in repository.</typeparam>
    internal class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly ISessionLocator _sessionLocator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repository{T}" /> class.
        /// </summary>
        public Repository(ISessionLocator sessionLocator)
        {
            Contract.Requires(sessionLocator != null);
            _sessionLocator = sessionLocator;
        }

        /// <summary>
        ///     Gets the query.
        /// </summary>
        public IQueryable<T> Query => Session.Query<T>().Cacheable();

        /// <summary>
        ///     Gets the current session.
        /// </summary>
        public ISession Session => _sessionLocator.GetSession();

        /// <summary>
        ///     Fetches all entities from the repository.
        /// </summary>
        /// <returns>A list of entities.</returns>
        public IList<T> GetAll()
        {
            return Query.ToList();
        }

        /// <summary>
        ///     Returns an entity which has the given identifier.
        /// </summary>
        /// <param name="id">The entity's identifier.</param>
        /// <returns>An entity or null.</returns>
        public T GetById(long id)
        {
            return Session.Get<T>(id);
        }

        /// <summary>
        ///     Fetches all entities which match the given predicate.
        /// </summary>
        /// <param name="predicate">A function which specifies a condition.</param>
        /// <returns>A list of entities.</returns>
        public IList<T> Filter(Expression<Func<T, bool>> predicate)
        {
            return Query.Where(predicate).ToList();
        }

        /// <summary>
        ///     Returns a number of entities in repository.
        /// </summary>
        /// <returns>A number of entities.</returns>
        public int GetCount()
        {
            return Query.Count();
        }

        /// <summary>
        ///     Returns a number of entities which matche the given predicate.
        /// </summary>
        /// <param name="predicate">A function which specifies a condition.</param>
        /// <returns>A number of entities.</returns>
        public int GetCount(Expression<Func<T, bool>> predicate)
        {
            return Query.Where(predicate).Count();
        }

        /// <summary>
        ///     Inserts the specified entity into repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Insert(T entity)
        {
            Session.SaveOrUpdate(entity);
        }

        /// <summary>
        ///     Updates the specified entity in repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(T entity)
        {
            Session.Update(entity);
        }

        /// <summary>
        ///     Deletes the specified entity from repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(T entity)
        {
            Session.Delete(entity);
        }
    }
}