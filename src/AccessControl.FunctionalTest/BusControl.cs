using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Service.Configuration;
using MassTransit;
using TechTalk.SpecFlow;

namespace AccessControl.FunctionalTest
{
    [Binding]
    public static class Bus
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

        public static IBusControl Instance
        {
            get
            {
                if (_busControl != null)
                    return _busControl;

                var serviceConfig = (IServiceConfig)ConfigurationManager.GetSection("service");
                var rabbitMqConfig = serviceConfig.RabbitMq;

                _busControl = MassTransit.Bus.Factory.CreateUsingRabbitMq(
                    cfg =>
                    {
                        cfg.UseJsonSerializer();
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

        public static TResponse Request<TRequest, TResponse>(string queueName, TRequest request)
            where TRequest : class
            where TResponse : class
        {
            var config = (IServiceConfig) ConfigurationManager.GetSection("service");
            IRequestClient<TRequest, TResponse> client = new MessageRequestClient<TRequest, TResponse>(Instance, new Uri(config.RabbitMq.GetQueueUrl(queueName)), TimeSpan.FromSeconds(30));
            return client.Request(request).Result;
        } 
    }
}