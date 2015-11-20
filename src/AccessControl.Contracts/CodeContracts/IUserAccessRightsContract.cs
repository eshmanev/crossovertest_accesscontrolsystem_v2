using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUserAccessRights" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUserAccessRights))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IUserAccessRightsContract : IUserAccessRights
    {
        public string UserName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public IPermanentAccessRule[] PermanentAccessRules
        {
            get
            {
                Contract.Ensures(Contract.Result<IPermanentAccessRule[]>() != null);
                return null;
            }
        }

        public IScheduledAccessRule[] ScheduledAccessRules
        {
            get
            {
                Contract.Ensures(Contract.Result<IScheduledAccessRule[]>() != null);
                return null;
            }
        }
    }
}