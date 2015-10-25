using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.Impl
{
    public class FindUserByName : IFindUserByName
    {
        public FindUserByName(string userName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            UserName = userName;
        }

        public string UserName { get; }
    }
}