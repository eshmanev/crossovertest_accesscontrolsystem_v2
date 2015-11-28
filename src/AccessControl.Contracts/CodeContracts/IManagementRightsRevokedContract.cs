using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IManagementRightsRevoked" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IManagementRightsRevoked))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IManagementRightsRevokedContract : IManagementRightsRevoked
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