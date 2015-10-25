using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.Impl
{
    public class GetPasswordHash : IGetPasswordHash
    {
        public GetPasswordHash(string userName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            UserName = userName;
        }
        public string UserName { get; }
    }
}