using System.Configuration;

namespace AccessControl.Server.Configuration
{
    public class ServerConfiguration : ConfigurationSection, IServerConfiguration
    {
        [ConfigurationProperty("rabbitMq", IsRequired = true)]
        public RabbitMqSection RabbitMq => (RabbitMqSection) base["rabbitMq"];

        [ConfigurationProperty("recreateDatabaseSchema", DefaultValue = false)]
        public bool RecreateDatabaseSchema => (bool) base["recreateDatabaseSchema"];

        IRabbitMqSection IServerConfiguration.RabbitMq => RabbitMq;
    }
}