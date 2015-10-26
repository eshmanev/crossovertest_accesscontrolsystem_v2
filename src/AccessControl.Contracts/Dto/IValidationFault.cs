using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    [ContractClass(typeof(IValidationFaultContract))]
    public interface IValidationFault
    {
        IValidationPropertyError[] Details { get; }
        string Summary { get; }
    }
}