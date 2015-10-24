using System.Configuration;

namespace AccessControl.Server.Configuration
{
    public class RabbitMqSection : ConfigurationElement, IRabbitMqSection
    {
        [ConfigurationProperty("url", DefaultValue = "rabbitmq://localhost")]
        public string Url => (string)base["url"];

        [ConfigurationProperty("userName", DefaultValue = "guest")]
        public string UserName => (string) base["userName"];

        [ConfigurationProperty("password", DefaultValue = "guest")]
        public string Password => (string)base["password"];
    }
}