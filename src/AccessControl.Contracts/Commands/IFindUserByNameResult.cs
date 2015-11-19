using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands
{
    /// <summary>
    ///     Represents a result of the <see cref="IFindUserByName" /> command.
    /// </summary>
    public interface IFindUserByNameResult
    {
        /// <summary>
        ///     Gets the user.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        IUser User { get; }
    }
}