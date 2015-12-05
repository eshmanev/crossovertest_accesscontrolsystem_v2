using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Principal;
using AccessControl.Contracts.Dto;

namespace AccessControl.Service.Security
{
    internal class ServicePrincipal : IPrincipal
    {
        private readonly string[] _roles;

        public ServicePrincipal(ITicket decryptedTicket, string encryptedTicket)
        {
            Contract.Requires(decryptedTicket != null);

            DecryptedTicket = decryptedTicket;
            EncryptedTicket = encryptedTicket;
            Identity = new ServiceIdentity(decryptedTicket.UserName);
            _roles = decryptedTicket.Roles;
        }

        public bool IsInRole(string role)
        {
            return _roles.Contains(role);
        }

        public IIdentity Identity { get; }

        public string EncryptedTicket { get; }

        public ITicket DecryptedTicket { get; }
    }
}