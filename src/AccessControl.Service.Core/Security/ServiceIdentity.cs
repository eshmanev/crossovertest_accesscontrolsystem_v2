using System.Security.Principal;

namespace AccessControl.Service.Security
{
    internal class ServiceIdentity : IIdentity
    {
        public ServiceIdentity(Contracts.Dto.IIdentity wrappedIdentity)
        {
            WrappedIdentity = wrappedIdentity;
        }

        public string Name => WrappedIdentity.UserName;

        public string AuthenticationType => "CrossContextAuth";

        public bool IsAuthenticated => WrappedIdentity.IsAuthenticated;

        public Contracts.Dto.IIdentity WrappedIdentity { get; }
    }
}