

namespace AccessControl.Contracts
{
    using System.Diagnostics.Contracts;
    using AccessControl.Contracts.CodeContracts;

    [ContractClass(typeof(IGetPasswordHashResultContract))]
    public interface IGetPasswordHashResult
    {
        string PasswordHash { get; } 
    }
}