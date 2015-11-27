using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Contracts;
using MassTransit;
using MassTransit.Pipeline;

namespace AccessControl.Service.Security
{
    /// <summary>
    ///     Propagates the specified authentication ticket to remote services.
    /// </summary>
    public class EncryptedTicketPropagator : ISendObserver, IPublishObserver
    {
        private readonly string _encryptedTicket;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EncryptedTicketPropagator" /> class.
        /// </summary>
        /// <param name="encryptedTicket">The encrypted ticket.</param>
        public EncryptedTicketPropagator(string encryptedTicket)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(encryptedTicket));
            _encryptedTicket = encryptedTicket;
        }

        public Task PreSend<T>(SendContext<T> context) where T : class
        {
            context.Headers.Set(WellKnownHeaders.Ticket, _encryptedTicket);
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
            context.Headers.Set(WellKnownHeaders.Ticket, _encryptedTicket);
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
    }
}