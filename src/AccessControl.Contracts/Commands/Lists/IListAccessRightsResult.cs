using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Lists
{
    /// <summary>
    ///     Represents a result of the <see cref="IListAccessRights" /> command.
    /// </summary>
    [ContractClass(typeof(IListAccessRightsResultContract))]
    public interface IListAccessRightsResult
    {
    }
}