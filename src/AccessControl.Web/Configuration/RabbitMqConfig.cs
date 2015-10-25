using System.Configuration;

namespace AccessControl.Web.Configuration
{
    public class RabbitMqConfig : ConfigurationSection, IRabbitMqConfig
    {
        [ConfigurationProperty("url", DefaultValue = "rabbitmq://localhost")]
        public string Url => (string)base["url"];

        [ConfigurationProperty("userName", DefaultValue = "guest")]
        public string UserName => (string) base["userName"];

        [ConfigurationProperty("password", DefaultValue = "guest")]
        public string Password => (string)base["password"];
    }
}