using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;
using AccessControl.Contracts.Impl.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUserAddedToGroup" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUserAddedToGroup))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IUserAddedToGroupContract : IUserAddedToGroup
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