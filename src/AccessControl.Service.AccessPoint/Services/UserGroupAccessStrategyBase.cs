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
    ///     An abstract base class for user group-specific strategies.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class UserGroupAccessStrategyBase<T> : IAccessManagementStrategy
        where T : AccessRuleBase, new()
    {
        private readonly IRequestClient<IFindUserGroupByName, IFindUserGroupByNameResult> _findGroupRequest;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserGroupAccessStrategyBase{T}" /> class.
        /// </summary>
        /// <param name="databaseContext">The database context.</param>
        /// <param name="findGroupRequest">The find group request.</param>
        /// <param name="groupName">Name of the group.</param>
        protected UserGroupAccessStrategyBase(IDatabaseContext databaseContext, IRequestClient<IFindUserGroupByName, IFindUserGroupByNameResult> findGroupRequest, string groupName)
        {
            Contract.Requires(databaseContext != null);
            Contract.Requires(findGroupRequest != null);
            Contract.Requires(groupName != null);

            DatabaseContext = databaseContext;
            _findGroupRequest = findGroupRequest;
            GroupName = groupName;
        }

        /// <summary>
        ///     Gets the name of the group.
        /// </summary>
        /// <value>
        ///     The name of the group.
        /// </value>
        public string GroupName { get; }

        /// <summary>
        ///     Gets the database context.
        /// </summary>
        /// <value>
        ///     The database context.
        /// </value>
        public IDatabaseContext DatabaseContext { get; }

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

            var userResult = await _findGroupRequest.Request(new FindUserGroupByName(GroupName));
            if (userResult == null)
            {
                return "Invalid user group name";
            }

            return null;
        }

        /// <summary>
        ///     Gets or creates the access rights.
        /// </summary>
        /// <returns></returns>
        public AccessRightsBase FindAccessRights()
        {
            var accessRights = DatabaseContext
                .AccessRights.Filter(x => x is UserGroupAccessRights && ((UserGroupAccessRights) x).UserGroupName == GroupName)
                .Cast<UserGroupAccessRights>()
                .FirstOrDefault();

            return accessRights;
        }

        /// <summary>
        ///     Creates a new access rights entity.
        /// </summary>
        /// <returns></returns>
        public AccessRightsBase CreateAccessRights()
        {
            return new UserGroupAccessRights {UserGroupName = GroupName};
        }

        /// <summary>
        /// Searches a rule for the specified access point.
        /// </summary>
        /// <param name="accessRights">The access rights.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public AccessRuleBase FindAccessRule(AccessRightsBase accessRights, Data.Entities.AccessPoint accessPoint)
        {
            return accessRights.AccessRules.OfType<PermanentAccessRule>().FirstOrDefault(x => x.AccessPoint == accessPoint);
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
            var hashEntity = DatabaseContext.Users.Filter(x => x.UserName == userName).SingleOrDefault();
            return hashEntity?.BiometricHash;
        }
    }
}