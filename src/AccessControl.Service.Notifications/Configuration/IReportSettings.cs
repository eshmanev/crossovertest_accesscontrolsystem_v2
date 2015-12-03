using System.Diagnostics.Contracts;
using AccessControl.Service.Notifications.CodeContracts;

namespace AccessControl.Service.Notifications.Configuration
{
    [ContractClass(typeof(IReportSettingsContract))]
    public interface IReportSettings
    {
        /// <summary>
        ///     Gets the hours when the system generates reports.
        /// </summary>
        /// <value>
        ///     The report hours.
        /// </value>
        byte Hours { get; }

        /// <summary>
        ///     Gets the minutes when the system generates reports.
        /// </summary>
        /// <value>
        ///     The report minutes.
        /// </value>
        byte Minutes { get; }
    }
}