using AccessControl.Contracts;
using AccessControl.Contracts.Dto;
using MassTransit;

namespace AccessControl.Service
{
    /// <summary>
    ///     Contains extension methods of the <see cref="ConsumeContext" />.
    /// </summary>
    public static class ConsumeContextExtensions
    {
        public static string Department(this ConsumeContext context)
        {
            return context.Headers.Get<IIdentity>(WellKnownHeaders.Identity)?.Department;
        }

        public static string Site(this ConsumeContext context)
        {
            return context.Headers.Get<IIdentity>(WellKnownHeaders.Identity)?.Site;
        }

        public static string UserName(this ConsumeContext context)
        {
            return context.Headers.Get<IIdentity>(WellKnownHeaders.Identity)?.UserName;
        }
    }
}