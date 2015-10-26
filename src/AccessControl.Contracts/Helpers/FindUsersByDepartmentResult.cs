using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class FindUsersByDepartmentResult : IFindUsersByDepartmentResult
    {
        public FindUsersByDepartmentResult(IUser[] users)
        {
            Contract.Requires(users != null);
            Users = users;
        }

        public IUser[] Users { get; }
    }
}