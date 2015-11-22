using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Lists
{
    [ContractClass(typeof(IListUsersInGroupResultContract))]
    public interface IListUsersInGroupResult
    {
        /// <summary>
        ///     Gets the users.
        /// </summary>
        /// <value>
        ///     The users.
        /// </value>
        IUser[] Users { get; }
    }
}