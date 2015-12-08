using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Data;
using AccessControl.Data.Entities;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Services
{
    /// <summary>
    ///     An abstract base class for user-specific strategies.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class UserAccessStrategyBase<T> : IAccessManagementStrategy
        where T : AccessRuleBase, new()
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IRequestClient<IFindUserByName, IFindUserByNameResult> _findUserRequest;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserAccessStrategyBase{T}" /> class.
        /// </summary>
        /// <param name="databaseContext">The database context.</param>
        /// <param name="findUserRequest">The find user request.</param>
        /// <param name="userName">Name of the user.</param>
        protected UserAccessStrategyBase(IDatabaseContext databaseContext, IRequestClient<IFindUserByName, IFindUserByNameResult> findUserRequest, string userName)
        {
            Contract.Requires(databaseContext != null);
            Contract.Requires(findUserRequest != null);
            Contract.Requires(userName != null);

            _databaseContext = databaseContext;
            _findUserRequest = findUserRequest;
            UserName = userName;
        }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get; }

        /// <summary>
        ///     Validates the request and returns an error message.
        /// </summary>
        /// <returns></returns>
        public async Task<string> ValidateRequest()
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                return "Not authorized";
            }

            var userResult = await _findUserRequest.Request(new FindUserByName(UserName));
            if (userResult == null)
            {
                return "Invalid user name";
            }

            return null;
        }

        /// <summary>
        ///     Gets or creates the access rights.
        /// </summary>
        /// <returns></returns>
        public AccessRightsBase FindAccessRights()
        {
            var accessRights = _databaseContext.AccessRights
                                               .Filter(x => x is UserAccessRights && ((UserAccessRights) x).UserName == UserName)
                                               .Cast<UserAccessRights>()
                                               .FirstOrDefault();
            return accessRights;
        }

        /// <summary>
        ///     Creates a new access rights entity.
        /// </summary>
        /// <returns></returns>
        public AccessRightsBase CreateAccessRights()
        {
            return new UserAccessRights {UserName = UserName};
        }

        /// <summary>
        /// Searches a rule for the specified access point.
        /// </summary>
        /// <param name="accessRights">The access rights.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public AccessRuleBase FindAccessRule(AccessRightsBase accessRights, Data.Entities.AccessPoint accessPoint)
        {
            return accessRights.AccessRules.OfType<T>().FirstOrDefault(x => x.AccessPoint == accessPoint);
        }

        /// <summary>
        ///     Creates a new access rule.
        /// </summary>
        /// <returns></returns>
        public virtual AccessRuleBase CreateAccessRule()
        {
            return new T();
        }

        /// <summary>
        ///     Publishes an access granted event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public abstract Task OnAccessGranted(IBus bus, Data.Entities.AccessPoint accessPoint);

        /// <summary>
        ///     Publishes an access denied event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public abstract Task OnAccessDenied(IBus bus, Data.Entities.AccessPoint accessPoint);

        /// <summary>
        ///     Finds the user hash.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        protected string FindUserHash(string userName)
        {
            var hashEntity = _databaseContext.Users.Filter(x => x.UserName == userName).SingleOrDefault();
            return hashEntity?.BiometricHash;
        }
    }
}