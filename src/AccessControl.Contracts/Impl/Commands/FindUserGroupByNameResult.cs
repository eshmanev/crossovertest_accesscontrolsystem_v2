using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Commands
{
    public class FindUserGroupByNameResult : IFindUserGroupByNameResult
    {
        public FindUserGroupByNameResult(IUserGroup userGroup)
        {
            UserGroup = userGroup;
        }

        public IUserGroup UserGroup { get; }
    }
}