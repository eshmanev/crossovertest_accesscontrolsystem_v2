using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IRevokeManagementRights" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IRevokeManagementRights))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IRevokeManagementRightsContract : IRevokeManagementRights
    {
        public string UserName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}