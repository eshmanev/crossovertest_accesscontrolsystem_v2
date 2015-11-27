using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.Helpers
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