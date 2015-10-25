

namespace AccessControl.Contracts
{
    using System.Diagnostics.Contracts;
    using AccessControl.Contracts.CodeContracts;

    [ContractClass(typeof(IFindUserByNameResultContract))]
    public interface IFindUserByNameResult
    {
        string UserName { get; }
        string Email { get; }
        string PhoneNumber { get; }
    }
}