using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Lists
{
    /// <summary>
    ///     Returns a list of users who are delegated management and monitoring rights
    /// </summary>
    [ContractClass(typeof(IListDelegatedUsersContract))]
    public interface IListDelegatedUsers
    {
    }
}