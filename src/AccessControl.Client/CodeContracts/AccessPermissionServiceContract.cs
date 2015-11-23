using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Client.Data;
using AccessControl.Client.Services;

namespace AccessControl.Client.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAccessPermissionService" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAccessPermissionService))]
    // ReSharper disable once InconsistentNaming
    internal abstract class AccessPermissionServiceContract : IAccessPermissionService
    {
        public void Load(IAccessPermissionCollection accessPermissions)
        {
            Contract.Requires(accessPermissions != null);
        }

        public void Save(IAccessPermissionCollection accessPermissions)
        {
            Contract.Requires(accessPermissions != null);
        }

        public Task<bool> Update(IAccessPermissionCollection accessPermissions)
        {
            Contract.Requires(accessPermissions != null);
            Contract.Ensures(Contract.Result<Task<bool>>() != null);
            return null;
        }
    }
}