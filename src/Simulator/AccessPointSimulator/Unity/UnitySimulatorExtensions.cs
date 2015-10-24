using System.ServiceModel;
using AccessPointSimulator.AccessCheckService;
using AccessPointSimulator.AccessPointRegistry;
using ChannelAdam.ServiceModel;
using Microsoft.Practices.Unity;

namespace AccessPointSimulator.Unity
{
    public class UnitySimulatorExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {

            Container

                .RegisterType<IServiceConsumer<IAccessCheckService>>(
                    new InjectionFactory(c => CreateConsumer<IAccessCheckService, AccessCheckServiceClient>()))

                .RegisterType<IServiceConsumer<IAccessPointRegistry>>(
                    new InjectionFactory(c => CreateConsumer<IAccessPointRegistry, AccessPointRegistryClient>()))

                ;
        }

        private IServiceConsumer<TService> CreateConsumer<TService, TClient>()
            where TClient : TService, ICommunicationObject, new()
        {
            var consumer = ServiceConsumerFactory.Create<TService>(() => new TClient());
            consumer.ExceptionBehaviourStrategy = WcfExceptionBehaviourStrategy.Instance;
            return consumer;
        }
    }
}