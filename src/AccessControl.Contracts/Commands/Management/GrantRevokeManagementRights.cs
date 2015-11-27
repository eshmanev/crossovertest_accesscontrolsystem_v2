using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.Commands.Management
{
    public class GrantRevokeManagementRights : IGrantManagementRights, IRevokeManagementRights
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GrantRevokeManagementRights" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public GrantRevokeManagementRights(string userName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            UserName = userName;
        }

        /// <summary>
        ///     Gets the name of the user who are granted managment rights.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get; }
    }
}