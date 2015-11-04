using AccessControl.Service.Core.Middleware;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Middleware
{
    /// <summary>
    ///     Provides extension methods for the pipe configurator.
    /// </summary>
    public static class PipeConfiguratorExtensions
    {
        /// <summary>
        ///     Manages NHibernate session transactions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurator"></param>
        public static void UseSessionTransaction<T>(this IPipeConfigurator<T> configurator)
            where T : class, PipeContext
        {
            configurator.AddPipeSpecification(new GenericPipeSpecification<T>(new SessionTransactionFilter<T>()));
        }
    }
}