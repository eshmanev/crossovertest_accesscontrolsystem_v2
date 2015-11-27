using MassTransit;

namespace AccessControl.Service.Security
{
    public static class BusControlExtensions
    {
        /// <summary>
        ///     Connects the thread principal to all requests sent through the bus.
        /// </summary>
        /// <param name="busControl">The bus control.</param>
        public static void ConnectThreadPrincipal(this IBusControl busControl)
        {
            var propagator = new PrincipalTicketPropagator();
            busControl.ConnectSendObserver(propagator);
            busControl.ConnectPublishObserver(propagator);
        }

        /// <summary>
        ///     Connects the given ticket to all requests sent through the bus.
        /// </summary>
        /// <param name="busControl">The bus control.</param>
        /// <param name="ticket">The ticket.</param>
        public static void ConnectTicket(this IBusControl busControl, string ticket)
        {
            var propagator = new EncryptedTicketPropagator(ticket);
            busControl.ConnectSendObserver(propagator);
            busControl.ConnectPublishObserver(propagator);
        }
    }
}