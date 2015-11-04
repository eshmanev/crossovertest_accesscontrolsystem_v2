using System.Threading.Tasks;
using MassTransit;
using MassTransit.Pipeline;

namespace AccessControl.Service.Core.Middleware
{
    /// <summary>
    /// Manages NHibernate sessions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SessionTransactionFilter<T> : IFilter<T>
        where T : class, PipeContext
    {
        public async Task Send(T context, IPipe<T> next)
        {
            await next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("transactionManager");
        }
    }
}