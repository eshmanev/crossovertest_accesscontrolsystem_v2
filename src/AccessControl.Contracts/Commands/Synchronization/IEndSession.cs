using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Synchronization
{
    [ContractClass(typeof(IEndSessionContract))]
    public interface IEndSession
    {
    }
}