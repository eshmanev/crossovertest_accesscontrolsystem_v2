using System;
using System.Diagnostics.Contracts;
using AccessControl.Server.Configuration;
using MassTransit;
using Topshelf;

namespace AccessControl.Server
{
    public class ServerService : ServiceControl
    {
        private readonly IServerConfiguration _configuration;
        private IBusControl _bus;

        public ServerService(IServerConfiguration configuration)
        {
            Contract.Requires(configuration != null);
            _configuration = configuration;
        }

        public bool Start(HostControl hostControl)
        {
            _bus = ConfigureBus();
            _bus.Start();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _bus?.Stop();
            return true;
        }

        private IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.UseBsonSerializer();

                var host = cfg.Host(new Uri(_configuration.RabbitMq.Url), h =>
                {
                    h.Username(_configuration.RabbitMq.UserName);
                    h.Password(_configuration.RabbitMq.Password);
                });

                cfg.ReceiveEndpoint(host, "event_queue", e =>
                {
                    //e.Handler<ValueEntered>(context =>
                    //    Console.Out.WriteLineAsync($"Value was entered: {context.Message.Value}"));
                });
            });
        }
    }
}