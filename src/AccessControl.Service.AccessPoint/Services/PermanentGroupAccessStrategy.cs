using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Contracts.Impl.Events;
using AccessControl.Data;
using AccessControl.Data.Entities;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Services
{
    internal class PermanentGroupAccessStrategy : UserGroupAccessStrategyBase<PermanentAccessRule>
    {
        private readonly IRequestClient<IListUsersInGroup, IListUsersInGroupResult> _listUsersInGroupRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermanentGroupAccessStrategy"/> class.
        /// </summary>
        /// <param name="databaseContext">The database context.</param>
        /// <param name="findGroupRequest">The find group request.</param>
        /// <param name="listUsersInGroupRequest">The list users in group request.</param>
        /// <param name="groupName">Name of the group.</param>
        public PermanentGroupAccessStrategy(IDatabaseContext databaseContext,
                                            IRequestClient<IFindUserGroupByName, IFindUserGroupByNameResult> findGroupRequest,
                                            IRequestClient<IListUsersInGroup, IListUsersInGroupResult> listUsersInGroupRequest,
                                            string groupName)
            : base(databaseContext, findGroupRequest, groupName)
        {
            Contract.Requires(listUsersInGroupRequest != null);
            _listUsersInGroupRequest = listUsersInGroupRequest;
        }

        /// <summary>
        /// Publishes the <see cref="PermanentUserGroupAccessAllowed"/> event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public override async Task OnAccessGranted(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            var usersInGroupResult = await _listUsersInGroupRequest.Request(ListCommand.ListUsersInGroup(GroupName));
            var userNames = usersInGroupResult.Users.Select(x => x.UserName).ToArray();
            var userHashes = new string[userNames.Length];
            for (var i = 0; i < userNames.Length; i++)
            {
                userHashes[i] = FindUserHash(userNames[i]);
            }

            await bus.Publish(new PermanentUserGroupAccessAllowed(accessPoint.AccessPointId, GroupName, userNames, userHashes));
        }

        /// <summary>
        /// Publishes the <see cref="PermanentUserGroupAccessDenied"/> event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public override Task OnAccessDenied(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            return bus.Publish(new PermanentUserGroupAccessDenied(accessPoint.AccessPointId, GroupName));
        }
    }
}