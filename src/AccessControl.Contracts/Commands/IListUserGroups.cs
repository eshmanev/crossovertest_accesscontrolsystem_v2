using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(ListUserGroupsContract))]
    public interface IListUserGroups
    {
        /// <summary>
        ///     Gets the department.
        /// </summary>
        string Department { get; }

        /// <summary>
        ///     Gets the site.
        /// </summary>
        string Site { get; }
    }
}