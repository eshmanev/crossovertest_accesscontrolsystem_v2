using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(IListAccessPointsContract))]
    public interface IListAccessPoints
    {
        string Department { get; }
        string Site { get; }
    }
}