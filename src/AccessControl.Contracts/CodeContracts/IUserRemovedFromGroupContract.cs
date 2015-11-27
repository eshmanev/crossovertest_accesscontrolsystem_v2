using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;
using AccessControl.Contracts.Impl.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUserRemovedFromGroup" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUserRemovedFromGroup))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IUserRemovedFromGroupContract : IUserRemovedFromGroup
    {
        public string UserGroupName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

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