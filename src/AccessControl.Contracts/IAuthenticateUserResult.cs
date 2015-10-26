

namespace AccessControl.Contracts
{
    using System.Diagnostics.Contracts;
    using AccessControl.Contracts.CodeContracts;

    [ContractClass(typeof(AuthenticateUserResultContract))]
    public interface IAuthenticateUserResult
    {
        bool Authenticated { get; }

        string Message { get; } 
    }
}