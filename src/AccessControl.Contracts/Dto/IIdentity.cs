using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    [ContractClass(typeof(IIdentityContract))]
    public interface IIdentity
    {
        /// <summary>
        /// Gets a value indicating whether the user is authenticated.
        /// </summary>
        /// <value>
        /// <c>true</c> if the user is authenticated; otherwise, <c>false</c>.
        /// </value>
        bool IsAuthenticated { get; }

        /// <summary>
        ///     Gets the department.
        /// </summary>
        /// <value>
        ///     The department.
        /// </value>
        string Department { get; }

        /// <summary>
        ///     Gets the site.
        /// </summary>
        /// <value>
        ///     The site.
        /// </value>
        string Site { get; }

        /// <summary>
        ///     Gets the name of the authenticated user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }
    }
}