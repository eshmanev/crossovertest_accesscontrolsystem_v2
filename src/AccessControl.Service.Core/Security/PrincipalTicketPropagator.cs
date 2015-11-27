using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using MassTransit;
using MassTransit.Pipeline;

namespace AccessControl.Service.Security
{
    /// <summary>
    ///     Propagates authentication ticket of the current principal to remote services.
    /// </summary>
    public class PrincipalTicketPropagator : ISendObserver, IPublishObserver
    {
        public Task PreSend<T>(SendContext<T> context) where T : class
        {
            AddHeaders(context);
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

        public Task PrePublish<T>(PublishContext<T> context) where T : class
        {
            AddHeaders(context);
            return Task.FromResult(true);
        }

        public Task PostPublish<T>(PublishContext<T> context) where T : class
        {
            return Task.FromResult(true);
        }

        public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
        {
            return Task.FromResult(true);
        }

        private void AddHeaders(SendContext context)
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                Debug.Assert(Thread.CurrentPrincipal is ServicePrincipal);

                // Single sign-on. Pass the encrypted ticket to the remote service.
                var principal = (ServicePrincipal)Thread.CurrentPrincipal;
                context.Headers.Set(WellKnownHeaders.Ticket, principal.EncryptedTicket);
            }
        }
    }
}