using System.Configuration;

namespace AccessControl.Service.Configuration
{
    /// <summary>
    ///     Represents a RabbitMQ configuration section.
    /// </summary>
    public class RabbitMqElement : ConfigurationElement, IRabbitMqConfig
    {
        /// <summary>
        ///     The URL
        /// </summary>
        [ConfigurationProperty("url", DefaultValue = "rabbitmq://localhost")]
        public string Url => (string) base["url"];

        /// <summary>
        ///     Gets or sets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        [ConfigurationProperty("userName", DefaultValue = "guest")]
        public string UserName => (string) base["userName"];

        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        /// <value>
        ///     The password.
        /// </value>
        [ConfigurationProperty("password", DefaultValue = "guest")]
        public string Password => (string) base["password"];

        /// <summary>
        ///     Gets the queue URL.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        public string GetQueueUrl(string queueName)
        {
            return Url.EndsWith("/") ? $"{Url}{queueName}" : $"{Url}/{queueName}";
        }
    }
}