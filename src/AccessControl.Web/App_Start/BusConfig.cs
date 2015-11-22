using System;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Web.Configuration;
using AccessControl.Web.Models.Account;
using MassTransit;
using MassTransit.Pipeline;
using Microsoft.Practices.Unity;

namespace AccessControl.Web
{
    public class BusConfig
    {
        public static IBusControl Configure(IUnityContainer container)
        {
            var rabbitMqConfig = UnityConfig.Container.Resolve<IRabbitMqConfig>();

            var busControl = Bus.Factory.CreateUsingRabbitMq(
                cfg =>
                {
                    cfg.UseBsonSerializer();
                    cfg.Host(
                        new Uri(rabbitMqConfig.Url),
                        h =>
                        {
                            h.Username(rabbitMqConfig.UserName);
                            h.Password(rabbitMqConfig.Password);
                        });
                });

            busControl.ConnectSendObserver(new MessageSendObserver());

            container.RegisterInstance<IBus>(busControl)
                     .RegisterInstance<IBusControl>(busControl);

            return busControl;
        }

        private class MessageSendObserver : ISendObserver
        {
            public Task PreSend<T>(SendContext<T> context) where T : class
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                {
                    var user = new ApplicationUser(Thread.CurrentPrincipal.Identity);
                    context.Headers.Set(WellKnownHeaders.Ticket, user.ServiceTicket);
                }
                return Task.FromResult(true);
            }

            public Task PostSend<T>(SendContext<T> context) where T : class
            {
                return Task.FromResult(true);
            }

            public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
            {
                return Task.FromResult(true);
            }
        }
    }
}