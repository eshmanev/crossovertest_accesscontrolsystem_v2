using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(AuthenticateUserContract))]
    public interface IAuthenticateUser
    {
        string UserName { get; }

        string Password { get; }
    }
}