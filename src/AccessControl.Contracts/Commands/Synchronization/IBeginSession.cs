using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using Microsoft.Synchronization;

namespace AccessControl.Contracts.Commands.Synchronization
{
    [ContractClass(typeof(IBeginSessionContract))]
    public interface IBeginSession
    {
    }
}