using System;
using System.Configuration;
using AccessControl.Client.Configuration;

namespace AccessControl.Client.Configuration
{
    public class ClientConfiguration : ConfigurationSection, IClientConfiguration
    {
        [ConfigurationProperty("rabbitMq", IsRequired = true)]
        public RabbitMqSection RabbitMq => (RabbitMqSection)base["rabbitMq"];

        IRabbitMqSection IClientConfiguration.RabbitMq => RabbitMq;
    }
}