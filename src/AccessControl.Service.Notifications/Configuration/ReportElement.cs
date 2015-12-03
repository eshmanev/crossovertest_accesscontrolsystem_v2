using System.Configuration;

namespace AccessControl.Service.Notifications.Configuration
{
    internal class ReportElement : ConfigurationElement, IReportSettings
    {
        /// <summary>
        ///     The hours
        /// </summary>
        [ConfigurationProperty("hours", IsRequired = true)]
        public byte Hours => (byte) base["hours"];

        /// <summary>
        ///     Gets or sets the minutes.
        /// </summary>
        /// <value>
        ///     The minutes.
        /// </value>
        [ConfigurationProperty("minutes", IsRequired = true)]
        public byte Minutes => (byte) base["minutes"];
    }
}