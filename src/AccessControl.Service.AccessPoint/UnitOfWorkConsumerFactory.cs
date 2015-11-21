using System.Threading.Tasks;
using AccessControl.Data;
using AccessControl.Data.Session;
using MassTransit;
using MassTransit.Pipeline;
using MassTransit.Util;
using Microsoft.Practices.Unity;

namespace AccessControl.Service.AccessPoint
{
    public class UnitOfWorkConsumerFactory<TConsumer> : IConsumerFactory<TConsumer> where TConsumer : class
    {
        private readonly IUnityContainer _container;

        public UnitOfWorkConsumerFactory(IUnityContainer container)
        {
            _container = container;
        }

        public async Task Send<T>(ConsumeContext<T> context, IPipe<ConsumerConsumeContext<TConsumer, T>> next)
            where T : class
        {
            using (var childContainer = _container.CreateChildContainer())
            using (var sessionScope = _container.Resolve<ISessionScopeFactory>().Create())
            {
                childContainer.RegisterInstance(sessionScope);
                childContainer.RegisterType(typeof(IRepository<>), typeof(Repository<>));

                var consumer = childContainer.Resolve<TConsumer>();
                if (consumer == null)
                {
                    throw new ConsumerException($"Unable to resolve consumer type '{TypeMetadataCache<TConsumer>.ShortName}'.");
                }

                try
                {
                    await next.Send(context.PushConsumer(consumer));
                    sessionScope.Commit();
                }
                catch
                {
                    sessionScope.Rollback();
                    throw;
                }
            }
        }

        void IProbeSite.Probe(ProbeContext context)
        {
            context.CreateConsumerFactoryScope<TConsumer>("unity");
        }
    }
}