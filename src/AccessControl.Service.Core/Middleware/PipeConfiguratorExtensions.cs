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
        public static void UseExceptionLogger<T>(this IPipeConfigurator<T> configurator)
            where T : class, PipeContext
        {
            configurator.AddPipeSpecification(new GenericPipeSpecification<T>(new ExceptionLoggerFilter<T>()));
        }

        public static void UseIdentity<T>(this IPipeConfigurator<T> configurator)
            where T : class, PipeContext
        {
            configurator.AddPipeSpecification(new GenericPipeSpecification<T>(new IdentityFilter<T>()));
        }
    }
}