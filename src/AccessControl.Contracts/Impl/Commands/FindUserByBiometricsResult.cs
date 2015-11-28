using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Commands
{
    public class FindUserByBiometricsResult : IFindUserByBiometricsResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FindUserByBiometricsResult" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public FindUserByBiometricsResult(IUserBiometric user)
        {
            User = user;
        }

        /// <summary>
        ///     Gets the user.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        public IUserBiometric User { get; }
    }
}