using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.Impl
{
    public class FindUserByNameResult : IFindUserByNameResult
    {
        public FindUserByNameResult(string userName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            UserName = userName;
        }

        public string UserName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
    }
}