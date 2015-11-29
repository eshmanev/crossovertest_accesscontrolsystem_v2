using System.Threading.Tasks;
using AccessControl.Data;
using AccessControl.Data.Session;
using MassTransit;
using MassTransit.Pipeline;
using MassTransit.Util;
using Microsoft.Practices.Unity;

namespace AccessControl.Service.AccessPoint
{
    public class DbContextConsumerFactory<TConsumer> : IConsumerFactory<TConsumer> where TConsumer : class
    {
        private readonly IUnityContainer _container;

        public DbContextConsumerFactory(IUnityContainer container)
        {
            _container = container;
        }

        public async Task Send<T>(ConsumeContext<T> context, IPipe<ConsumerConsumeContext<TConsumer, T>> next)
            where T : class
        {
            using (var childContainer = _container.CreateChildContainer())
            using (var databaseContext = new DatabaseContext(childContainer.Resolve<ISessionFactoryHolder>()))
            {
                childContainer.RegisterInstance<IDatabaseContext>(databaseContext);
                var consumer = childContainer.Resolve<TConsumer>();
                if (consumer == null)
                {
                    throw new ConsumerException($"Unable to resolve consumer type '{TypeMetadataCache<TConsumer>.ShortName}'.");
                }

                await next.Send(context.PushConsumer(consumer));
                databaseContext.Commit();
            }
        }

        void IProbeSite.Probe(ProbeContext context)
        {
            context.CreateConsumerFactoryScope<TConsumer>("unity");
        }
    }
}