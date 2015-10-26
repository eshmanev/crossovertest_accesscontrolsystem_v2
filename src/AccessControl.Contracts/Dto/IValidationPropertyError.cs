using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    [ContractClass(typeof(ValidationPropertyErrorContract))]
    public interface IValidationPropertyError
    {
        string Message { get; }
        string PropertyName { get; }
    }
}