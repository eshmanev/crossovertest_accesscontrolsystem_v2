using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Commands
{
    public class FindUserByNameResult : IFindUserByNameResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FindUserByNameResult" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public FindUserByNameResult(IUser user)
        {
            User = user;
        }

        /// <summary>
        ///     Gets the user.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        public IUser User { get; }
    }
}