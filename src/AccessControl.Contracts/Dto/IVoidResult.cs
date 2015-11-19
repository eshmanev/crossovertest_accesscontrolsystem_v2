using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    [ContractClass(typeof(IVoidResultContract))]
    public interface IVoidResult
    {
        bool Succeded { get; }
        IValidationFault Fault { get; }
    }
}