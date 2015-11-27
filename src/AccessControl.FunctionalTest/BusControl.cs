using System;
using System.Configuration;
using AccessControl.Service.Configuration;
using MassTransit;
using TechTalk.SpecFlow;

namespace AccessControl.FunctionalTest
{
    [Binding]
    public class BusControl
    {
        private static IBusControl _busControl;

        [AfterFeature]
        public static void Cleanup()
        {
            if (_busControl != null)
            {
                _busControl.Stop();
                _busControl = null;
            }
        }

        public static IBusControl GetBus()
        {
            if (_busControl != null)
                return _busControl;

            var serviceConfig = (IServiceConfig) ConfigurationManager.GetSection("service");
            var rabbitMqConfig = serviceConfig.RabbitMq;

            _busControl = Bus.Factory.CreateUsingRabbitMq(
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
            _busControl.Start();
            return _busControl;
        }
    }
}