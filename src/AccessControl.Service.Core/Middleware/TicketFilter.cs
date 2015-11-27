using System.Diagnostics.Contracts;
using System.Security.Principal;
using AccessControl.Contracts;
using AccessControl.Service.Security;
using MassTransit;
using MassTransit.Pipeline;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts.Impl.Dto;

namespace AccessControl.Service.Middleware
{
    internal class TicketFilter<T> : IFilter<T>
        where T : class, PipeContext
    {
        private readonly IEncryptor _encryptor;

        public TicketFilter(IEncryptor encryptor)
        {
            Contract.Requires(encryptor != null);
            _encryptor = encryptor;
        }

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

            var encryptedTicket = messageContext.Headers.Get<string>(WellKnownHeaders.Ticket);
            if (string.IsNullOrWhiteSpace(encryptedTicket))
                return Thread.CurrentPrincipal;

            var ticket = _encryptor.Decrypt<Ticket>(encryptedTicket);
            return new ServicePrincipal(ticket, encryptedTicket);
        }
    }
}