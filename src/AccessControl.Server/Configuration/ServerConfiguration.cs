using System.Configuration;

namespace AccessControl.Server.Configuration
{
    public class ServerConfiguration : ConfigurationSection, IServerConfiguration
    {
        [ConfigurationProperty("rabbitMq", IsRequired = true)]
        public RabbitMqSection RabbitMq => (RabbitMqSection) base["rabbitMq"];

        IRabbitMqSection IServerConfiguration.RabbitMq => RabbitMq;
    }
}