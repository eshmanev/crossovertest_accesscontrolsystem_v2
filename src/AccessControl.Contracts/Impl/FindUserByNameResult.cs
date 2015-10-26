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
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}