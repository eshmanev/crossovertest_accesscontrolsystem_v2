using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Service.Security;
using MassTransit;
using MassTransit.Pipeline;

namespace AccessControl.Service.Middleware
{
    internal class SendObserver : ISendObserver
    {
        public Task PreSend<T>(SendContext<T> context) where T : class
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                Debug.Assert(Thread.CurrentPrincipal is ServicePrincipal);

                // Single sign-on. Pass the encrypted ticket to the remote service.
                var principal = (ServicePrincipal) Thread.CurrentPrincipal;
                context.Headers.Set(WellKnownHeaders.Ticket, principal.EncryptedTicket);
            }
            
            return Task.FromResult(true);
        }

        public Task PostSend<T>(SendContext<T> context) where T : class
        {
            return Task.FromResult(true);
        }

        public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
        {
            return Task.FromResult(true);
        }
    }
}