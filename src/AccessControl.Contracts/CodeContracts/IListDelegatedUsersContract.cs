using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Lists;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListDelegatedUsers" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListDelegatedUsers))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListDelegatedUsersContract : IListDelegatedUsers
    {
    }
}