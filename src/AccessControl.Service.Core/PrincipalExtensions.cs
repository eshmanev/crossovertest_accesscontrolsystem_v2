using System.Security.Principal;
using AccessControl.Service.Security;

namespace AccessControl.Service
{
    /// <summary>
    ///     Contains extension methods of the <see cref="IPrincipal" />.
    /// </summary>
    public static class PrincipalExtensions
    {
        public static string Domain(this IPrincipal principal)
        {
            return principal.Cast()?.DecryptedTicket.Domain;
        }

        public static string Department(this IPrincipal principal)
        {
            return principal.Cast()?.DecryptedTicket.User.Department;
        }

        public static string[] OnBehalfOf(this IPrincipal principal)
        {
            return principal.Cast()?.DecryptedTicket.OnBehalfOf ?? new string[0];
        }

        public static string Site(this IPrincipal principal)
        {
            return principal.Cast()?.DecryptedTicket.User.Site;
        }

        public static string UserName(this IPrincipal principal)
        {
            return principal.Cast()?.DecryptedTicket.User.UserName;
        }

        private static ServicePrincipal Cast(this IPrincipal principal)
        {
            return principal as ServicePrincipal;
        }
    }
}