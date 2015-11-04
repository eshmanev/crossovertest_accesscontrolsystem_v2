using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace AccessControl.Data.Session
{
    /// <summary>
    ///     Represents a generic repository.
    /// </summary>
    /// <typeparam name="T">The type of entities in repository.</typeparam>
    internal class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly ISessionFactoryHolder _sessionFactoryHolder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repository{T}" /> class.
        /// </summary>
        public Repository(ISessionFactoryHolder sessionFactoryHolder)
        {
            Contract.Requires(sessionFactoryHolder != null);
            _sessionFactoryHolder = sessionFactoryHolder;
        }

        /// <summary>
        ///     Fetches all entities from the repository.
        /// </summary>
        /// <returns>A list of entities.</returns>
        public IList<T> GetAll()
        {
            return Query(x => x.ToList());
        }

        /// <summary>
        ///     Returns an entity which has the given identifier.
        /// </summary>
        /// <param name="id">The entity's identifier.</param>
        /// <returns>An entity or null.</returns>
        public T GetById(object id)
        {
            return Session(x => x.Get<T>(id));
        }

        /// <summary>
        ///     Fetches all entities which match the given predicate.
        /// </summary>
        /// <param name="predicate">A function which specifies a condition.</param>
        /// <returns>A list of entities.</returns>
        public IList<T> Filter(Expression<Func<T, bool>> predicate)
        {
            return Query(x => x.Where(predicate).ToList());
        }

        /// <summary>
        ///     Returns a number of entities in repository.
        /// </summary>
        /// <returns>A number of entities.</returns>
        public int GetCount()
        {
            return Query(x => x.Count());
        }

        /// <summary>
        ///     Returns a number of entities which matche the given predicate.
        /// </summary>
        /// <param name="predicate">A function which specifies a condition.</param>
        /// <returns>A number of entities.</returns>
        public int GetCount(Expression<Func<T, bool>> predicate)
        {
            return Query(x => x.Where(predicate).Count());
        }

        /// <summary>
        ///     Inserts the specified entity into repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Insert(T entity)
        {
            Transaction(x => x.SaveOrUpdate(entity));
        }

        /// <summary>
        ///     Updates the specified entity in repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(T entity)
        {
            Transaction(x => x.Update(entity));
        }

        /// <summary>
        ///     Deletes the specified entity from repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(T entity)
        {
            Transaction(x => x.Delete(entity));
        }

        private TResult Session<TResult>(Func<ISession, TResult> action)
        {
            using (var session = OpenSession())
            {
                return action(session);
            }
        }

        private TResult Query<TResult>(Func<IQueryable<T>, TResult> action)
        {
            return Session(
                x =>
                {
                    var query = x.Query<T>().Cacheable();
                    return action(query);
                });
        }

        private void Transaction(Action<ISession> action)
        {
            Session(
                x =>
                {
                    using (var transaction = x.BeginTransaction())
                    {
                        try
                        {
                            action(x);

                            if (transaction.IsActive)
                                transaction.Commit();
                        }
                        catch (Exception)
                        {
                            if (transaction.IsActive)
                                transaction.Commit();
                        }
                    }
                    return true;
                });
        }

        private ISession OpenSession()
        {
            return _sessionFactoryHolder.SessionFactory.OpenSession();
        }
    }
}