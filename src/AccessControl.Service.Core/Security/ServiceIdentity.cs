using System.Diagnostics.Contracts;
using System.Security.Principal;

namespace AccessControl.Service.Security
{
    internal class ServiceIdentity : IIdentity
    {
        public ServiceIdentity(string userName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Name = userName;
        }

        public string Name { get; }

        public string AuthenticationType => "AUTH";

        public bool IsAuthenticated => true;
    }
}