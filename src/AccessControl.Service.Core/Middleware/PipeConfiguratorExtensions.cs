using AccessControl.Service.Security;
using MassTransit;

namespace AccessControl.Service.Middleware
{
    /// <summary>
    ///     Provides extension methods for the pipe configurator.
    /// </summary>
    public static class PipeConfiguratorExtensions
    {
        /// <summary>
        ///     Logs pipe exceptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurator"></param>
        public static void UseUnhandledExceptionLogger<T>(this IPipeConfigurator<T> configurator)
            where T : class, PipeContext
        {
            configurator.AddPipeSpecification(new GenericPipeSpecification<T>(new ExceptionLoggerFilter<T>()));
        }

        public static void UseTickets<T>(this IPipeConfigurator<T> configurator, IEncryptor encryptor)
            where T : class, PipeContext
        {
            configurator.AddPipeSpecification(new GenericPipeSpecification<T>(new TicketFilter<T>(encryptor)));
        }
    }
}