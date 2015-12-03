using System;
using System.Diagnostics.Contracts;
using Twilio;

namespace AccessControl.Service.Notifications.Services
{
    /// <summary>
    ///     Represents a Twilio exception.
    /// </summary>
    public class SmsException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SmsException" /> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public SmsException(RestException error)
        {
            Contract.Requires(error != null);
            Error = error;
        }

        /// <summary>
        ///     Gets the error.
        /// </summary>
        /// <value>
        ///     The error.
        /// </value>
        public RestException Error { get; }
    }
}