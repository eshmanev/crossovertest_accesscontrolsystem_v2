using System.Security.Principal;
using AccessControl.Contracts;
using AccessControl.Contracts.Helpers;
using AccessControl.Service.Security;
using MassTransit;
using MassTransit.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControl.Service.Middleware
{
    internal class IdentityFilter<T> : IFilter<T>
        where T : class, PipeContext
    {
        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("exceptionLogger");
        }

        public async Task Send(T context, IPipe<T> next)
        {
            var principal = Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = CreatePrincipal(context);

            try
            {
                await next.Send(context);
            }
            finally
            {
                Thread.CurrentPrincipal = principal;
            }
        }

        private IPrincipal CreatePrincipal(T context)
        {
            var messageContext = context as MessageContext;
            if (messageContext == null)
                return null;

            var identity = messageContext.Headers.Get(WellKnownHeaders.Identity, Identity.Empty);
            if (!identity.IsAuthenticated)
                return null;

            return new ServicePrincipal(identity);
        }
    }
}