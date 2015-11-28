using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using AccessControl.Contracts;

namespace AccessControl.Service.Security
{
    public delegate IEnumerable<TData> GetAllData<out TData>();

    public delegate IEnumerable<TData> GetManagedData<out TData>(string managerName);

    /// <summary>
    ///     Represents a factory of role based data fetchers.
    /// </summary>
    public class RoleBasedDataFetcher
    {
        /// <summary>
        ///     Creates a new role based data fetcher.
        /// </summary>
        /// <typeparam name="TData">The type of fetched data.</typeparam>
        /// <param name="getAll">A function used to fetch all data.</param>
        /// <param name="getManaged">A function used to fetch data managed by the specified manager.</param>
        /// <returns></returns>
        public static RoleBasedDataFetcher<TData> Create<TData>(GetAllData<TData> getAll, GetManagedData<TData> getManaged)
        {
            Contract.Requires(getAll != null);
            Contract.Requires(getManaged != null);
            return new RoleBasedDataFetcher<TData>(getAll, getManaged);
        }
    }


    /// <summary>
    ///     Represents a servant used to fetch data depending on roles of the current principal.
    /// </summary>
    /// <typeparam name="TData">The type of fetched data.</typeparam>
    public class RoleBasedDataFetcher<TData>
    {
        private readonly GetAllData<TData> _getAll;
        private readonly GetManagedData<TData> _getManaged;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RoleBasedDataFetcher{TData}" /> class.
        /// </summary>
        /// <param name="getAll">The get all.</param>
        /// <param name="getManaged">The get managed.</param>
        internal RoleBasedDataFetcher(GetAllData<TData> getAll, GetManagedData<TData> getManaged)
        {
            Contract.Requires(getAll != null);
            Contract.Requires(getManaged != null);

            _getAll = getAll;
            _getManaged = getManaged;
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TData> Execute()
        {
            if (Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                var managers = new List<string> {Thread.CurrentPrincipal.UserName()};
                managers.AddRange(Thread.CurrentPrincipal.OnBehalfOf());
                return managers.SelectMany(x => _getManaged(x));
            }
            if (Thread.CurrentPrincipal.IsInRole(WellKnownRoles.ClientService))
            {
                return _getAll();
            }

            return Enumerable.Empty<TData>();
        }
    }
}