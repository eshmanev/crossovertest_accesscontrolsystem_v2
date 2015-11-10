using System.Threading.Tasks;
using AccessControl.Data;
using AccessControl.Data.Session;
using MassTransit;
using MassTransit.Pipeline;
using MassTransit.Util;
using Microsoft.Practices.Unity;

namespace AccessControl.Service.AccessPoint
{
    public class UnityConsumerFactory<TConsumer> : IConsumerFactory<TConsumer> where TConsumer : class
    {
        private readonly IUnityContainer _container;

        public UnityConsumerFactory(IUnityContainer container)
        {
            _container = container;
        }

        public async Task Send<T>(ConsumeContext<T> context, IPipe<ConsumerConsumeContext<TConsumer, T>> next)
            where T : class
        {
            using (var childContainer = _container.CreateChildContainer())
            {
                using (var unitOfWork = new SessionHolder(childContainer.Resolve<ISessionFactoryHolder>()))
                {
                    childContainer.RegisterInstance<ISessionHolder>(unitOfWork);
                    childContainer.RegisterType(typeof(IRepository<>), typeof(Repository<>));

                    var consumer = childContainer.Resolve<TConsumer>();
                    if (consumer == null)
                    {
                        throw new ConsumerException($"Unable to resolve consumer type '{TypeMetadataCache<TConsumer>.ShortName}'.");
                    }

                    try
                    {
                        await next.Send(context.PushConsumer(consumer));
                        unitOfWork.Commit();
                    }
                    catch
                    {
                        unitOfWork.Rollback();
                        throw;
                    }
                }
            }
        }

        void IProbeSite.Probe(ProbeContext context)
        {
            context.CreateConsumerFactoryScope<TConsumer>("unity");
        }
    }
}