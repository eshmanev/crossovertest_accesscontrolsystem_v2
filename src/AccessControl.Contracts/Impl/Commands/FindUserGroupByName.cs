using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Search;

namespace AccessControl.Contracts.Impl.Commands
{
    public class FindUserGroupByName : IFindUserGroupByName
    {
        public FindUserGroupByName(string userGroup)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userGroup));
            UserGroup = userGroup;
        }

        public string UserGroup { get; }
    }
}