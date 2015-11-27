using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IGrantManagementRights" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IGrantManagementRights))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IGrantManagementRightsContract : IGrantManagementRights
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