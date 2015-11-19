using System.Security.Principal;

namespace AccessControl.Service.Security
{
    internal class ServicePrincipal : IPrincipal
    {
        public ServicePrincipal(Contracts.Dto.IIdentity identity)
        {
            Identity = new ServiceIdentity(identity);
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        public System.Security.Principal.IIdentity Identity { get; }
    }
}