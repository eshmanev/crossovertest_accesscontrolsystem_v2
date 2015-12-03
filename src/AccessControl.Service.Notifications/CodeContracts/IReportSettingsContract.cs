using System.Diagnostics.Contracts;
using AccessControl.Service.Notifications.Configuration;

namespace AccessControl.Service.Notifications.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IReportSettings" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IReportSettings))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IReportSettingsContract : IReportSettings
    {
        public byte Hours
        {
            get
            {
                Contract.Ensures(Contract.Result<byte>() <= 23);
                return 0;
            }
        }

        public byte Minutes
        {
            get
            {
                Contract.Ensures(Contract.Result<byte>() <= 59);
                return 0;
            }
        }
    }
}