using System.Threading;
using AccessControl.Service.Security;
using MassTransit;

namespace AccessControl.Service
{
    /// <summary>
    ///     Contains extension methods of the <see cref="ConsumeContext" />.
    /// </summary>
    public static class ConsumeContextExtensions
    {
        private static ServicePrincipal Principal => Thread.CurrentPrincipal as ServicePrincipal;

        public static string Department(this ConsumeContext context)
        {
            return Principal?.DecryptedTicket.User.Department;
        }

        public static string Site(this ConsumeContext context)
        {
            return Principal?.DecryptedTicket.User.Site;
        }

        public static string UserName(this ConsumeContext context)
        {
            return Principal?.DecryptedTicket.User.UserName;
        }
    }
}