using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class FindUserByNameResult : IFindUserByNameResult
    {
        public FindUserByNameResult(IUser user)
        {
            User = user;
        }

        public IUser User { get; }
    }
}