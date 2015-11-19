using System.Diagnostics.Contracts;
using AccessControl.Service.CodeContracts;

namespace AccessControl.Service.Configuration
{
    [ContractClass(typeof(IRabbitMqConfigContract))]
    public interface IRabbitMqConfig
    {
        /// <summary>
        ///     Gets the password.
        /// </summary>
        /// <value>
        ///     The password.
        /// </value>
        string Password { get; }

        /// <summary>
        ///     Gets the server URL.
        /// </summary>
        /// <value>
        ///     The URL.
        /// </value>
        string Url { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }

        /// <summary>
        ///     Gets the queue URL.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        string GetQueueUrl(string queueName);
    }
}