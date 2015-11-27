using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Commands
{
    public class CheckCredentialsResult : ICheckCredentialsResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CheckCredentialsResult" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public CheckCredentialsResult(IUser user)
        {
            Valid = user != null;
            User = user;
        }

        /// <summary>
        ///     If credentials are valid this property contains the authenticated user; otherwise, null.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        public IUser User { get; }

        /// <summary>
        ///     Gets a value indicating whether the credentials are valid.
        /// </summary>
        public bool Valid { get; }
    }
}