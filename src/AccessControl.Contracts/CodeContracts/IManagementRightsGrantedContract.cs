using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IManagementRightsGranted" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IManagementRightsGranted))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IManagementRightsGrantedContract : IManagementRightsGranted
    {
        public string Grantee
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public string Grantor
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}