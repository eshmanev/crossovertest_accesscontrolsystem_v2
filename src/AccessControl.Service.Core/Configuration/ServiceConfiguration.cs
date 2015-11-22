using System;
using System.Configuration;

namespace AccessControl.Service.Configuration
{
    public class ServiceConfiguration : ConfigurationSection, IServiceConfig
    {
        /// <summary>
        ///     Gets the rabbit mq configuration.
        /// </summary>
        /// <value>
        ///     The rabbit mq configuration.
        /// </value>
        [ConfigurationProperty("rabbitMq")]
        public RabbitMqElement RabbitMq => (RabbitMqElement) base["rabbitMq"];

        /// <summary>
        ///     Gets the security configuration.
        /// </summary>
        /// <value>
        ///     The security configuration.
        /// </value>
        [ConfigurationProperty("security")]
        public SecurityElement Security => (SecurityElement) base["security"];

        IRabbitMqConfig IServiceConfig.RabbitMq => RabbitMq;

        ISecurityConfig IServiceConfig.Security => Security;
    }
}