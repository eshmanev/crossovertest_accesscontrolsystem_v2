using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(IListAccessRightsResultContract))]
    public interface IListAccessRightsResult
    {
    }
}