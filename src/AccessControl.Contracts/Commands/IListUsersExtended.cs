using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(ListUsersExtendedContract))]
    public interface IListUsersExtended
    {
        string Department { get; }
        string Site { get; }
    }
}