using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class ListUsersExtendedResult : IListUsersExtendedResult
    {
        public ListUsersExtendedResult(IUserExtended[] users)
        {
            Contract.Requires(users != null);
            Users = users;
        }

        public IUserExtended[] Users { get; }
    }
}