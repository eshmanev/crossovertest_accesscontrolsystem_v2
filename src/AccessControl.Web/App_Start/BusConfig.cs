using System;
using AccessControl.Web.Configuration;
using MassTransit;
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

                  var host = cfg.Host(
                      new Uri(rabbitMqConfig.Url),
                      h =>
                      {
                          h.Username(rabbitMqConfig.UserName);
                          h.Password(rabbitMqConfig.Password);
                      });
               });

            container.RegisterInstance<IBus>(busControl)
                     .RegisterInstance<IBusControl>(busControl);

            return busControl;
        }
    }
}