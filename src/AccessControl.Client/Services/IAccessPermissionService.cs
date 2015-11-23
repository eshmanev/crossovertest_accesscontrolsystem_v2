using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Client.CodeContracts;
using AccessControl.Client.Data;

namespace AccessControl.Client.Services
{
    [ContractClass(typeof(AccessPermissionServiceContract))]
    internal interface IAccessPermissionService
    {
        /// <summary>
        ///     Loads a saved state to the specified access permissions collection.
        /// </summary>
        /// <param name="accessPermissions">The access permissions.</param>
        void Load(IAccessPermissionCollection accessPermissions);

        /// <summary>
        ///     Saves the current state of the specified access permissions.
        /// </summary>
        /// <param name="accessPermissions">The access permissions.</param>
        void Save(IAccessPermissionCollection accessPermissions);

        /// <summary>
        ///     Updates the specified access permissions from server.
        /// </summary>
        /// <param name="accessPermissions">The access permissions.</param>
        /// <returns>true if update completes successfully, otherwise, false.</returns>
        Task<bool> Update(IAccessPermissionCollection accessPermissions);
    }
}