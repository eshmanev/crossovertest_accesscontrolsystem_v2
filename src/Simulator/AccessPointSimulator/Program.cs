using System;
using AccessPointSimulator.Dialog;
using AccessPointSimulator.Unity;
using ChannelAdam.ServiceModel;
using Microsoft.Practices.Unity;
using Vendor.API;
using IAccessPointRegistry = AccessPointSimulator.AccessPointRegistry.IAccessPointRegistry;

namespace AccessPointSimulator
{
    public class Program
    {
        private static IUnityContainer Container;

        public static void Main(params string[] args)
        {
            Container = new UnityContainer();
            Container.AddExtension(new UnitySimulatorExtension());

            log4net.Config.XmlConfigurator.Configure();

            ProgramOption action;
            while (Options.SelectOption(out action))
            {
                switch (action)
                {
                    case ProgramOption.RegisterAccessPoint:
                        Options.With<AccessPointParameters>(RegisterAccessPoint);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void ProcessResult(IOperationResult result)
        {
            if (result.HasNoException)
            {
                Console.WriteLine("Operation succeeded.");
            }
            if (result.HasFaultException)
            {
                Console.WriteLine("Service operation threw a fault. See log for details.");
            }
            else if (result.HasException)
            {
                Console.WriteLine("An unexpected error occurred while calling the service operation. See log for details.");
            }
        }

        private static void RegisterAccessPoint(AccessPointParameters parameters)
        {
            using (var consumer = Container.Resolve<IServiceConsumer<IAccessPointRegistry>>())
            {
                var dto = new AccessPointDto {AccessPointId = parameters.Id, Name = parameters.Name, Description = parameters.Description};
                var result = consumer.Consume(x => x.RegisterAccessPoint(dto));
                ProcessResult(result);
            }
        }
    }
}