using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(AuthenticateUserResultContract))]
    public interface IAuthenticateUserResult
    {
        bool Authenticated { get; }
        string Message { get; }
    }
}