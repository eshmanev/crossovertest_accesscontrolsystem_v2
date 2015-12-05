using System.Security.Principal;
using AccessControl.Service.Security;

namespace AccessControl.Service
{
    /// <summary>
    ///     Contains extension methods of the <see cref="IPrincipal" />.
    /// </summary>
    public static class PrincipalExtensions
    {
        public static string[] OnBehalfOf(this IPrincipal principal)
        {
            return principal.Cast()?.DecryptedTicket.OnBehalfOf ?? new string[0];
        }

        public static string UserName(this IPrincipal principal)
        {
            return principal.Cast()?.DecryptedTicket.UserName;
        }

        private static ServicePrincipal Cast(this IPrincipal principal)
        {
            return principal as ServicePrincipal;
        }
    }
}