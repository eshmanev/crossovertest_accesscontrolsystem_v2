namespace AccessControl.Service.Configuration
{
    public interface IServiceConfig
    {
        /// <summary>
        ///     Gets the rabbit mq configuration.
        /// </summary>
        /// <value>
        ///     The rabbit mq configuration.
        /// </value>
        IRabbitMqConfig RabbitMq { get; }

        /// <summary>
        ///     Gets the security configuration.
        /// </summary>
        /// <value>
        ///     The security configuration.
        /// </value>
        ISecurityConfig Security { get; }
    }
}