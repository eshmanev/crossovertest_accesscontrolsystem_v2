using System;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MassTransit;
using MassTransit.Pipeline;

namespace AccessControl.Service.Core.Middleware
{
    /// <summary>
    /// Logs pipe exceptions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ExceptionLoggerFilter<T> : IFilter<T>
        where T : class, PipeContext
    {
        private static ILog Log = LogManager.GetLogger(typeof(ExceptionLoggerFilter<T>));

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("exceptionLogger");
        }

        public async Task Send(T context, IPipe<T> next)
        {
            try
            {
                await next.Send(context);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing a message", ex);
                throw;
            }
        }
    }
}