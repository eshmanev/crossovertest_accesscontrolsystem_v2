using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class ListUserGroupsResult : IListUserGroupsResult
    {
        public ListUserGroupsResult(IUserGroup[] groups)
        {
            Contract.Requires(groups != null);
            Groups = groups;
        }

        public IUserGroup[] Groups { get; }
    }
}