using System;
using System.Diagnostics.Contracts;
using MassTransit;
using Topshelf;

namespace AccessControl.Service
{
    /// <summary>
    ///     Represents a bus service control.
    /// </summary>
    public class BusServiceControl : ServiceControl
    {
        private readonly IBusControl _bus;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BusServiceControl" /> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        public BusServiceControl(IBusControl bus)
        {
            Contract.Requires(bus != null);
            _bus = bus;
        }

        /// <summary>
        ///     Starts the service..
        /// </summary>
        /// <param name="hostControl">The host control.</param>
        /// <returns></returns>
        public virtual bool Start(HostControl hostControl)
        {
            try
            {
                _bus.Start();
                return true;
            }
            catch (Exception e)
            {
                LogFatal("An error occurred during starting the service", e);
                return false;
            }
        }

        /// <summary>
        ///     Stops the service.
        /// </summary>
        /// <param name="hostControl">The host control.</param>
        /// <returns></returns>
        public virtual bool Stop(HostControl hostControl)
        {
            _bus.Stop();
            return true;
        }

        /// <summary>
        ///     Logs the fatal error to the system event log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The e.</param>
        protected void LogFatal(string message, Exception e)
        {
            var log = Topshelf.Logging.HostLogger.Get(GetType());
            log.Fatal(message, e);
        }

        /// <summary>
        ///     Logs the specified error to the system event log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The e.</param>
        protected void LogError(string message, Exception e)
        {
            var log = Topshelf.Logging.HostLogger.Get(GetType());
            log.Error(message, e);
        }
    }
}