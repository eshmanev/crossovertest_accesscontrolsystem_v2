using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Search;

namespace AccessControl.Contracts.Impl.Commands
{
    public class FindUserByName : IFindUserByName
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FindUserByName" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public FindUserByName(string userName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            UserName = userName;
        }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get; }
    }
}