using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(ListUsersExtendedResultContract))]
    public interface IListUsersExtendedResult
    {
        IUserExtended[] Users { get; }
    }
}