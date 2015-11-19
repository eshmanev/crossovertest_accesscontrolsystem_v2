using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    [ContractClass(typeof(IUserContract))]
    public interface IUser
    {
        string Site { get; }
        string Department { get; }
        string DisplayName { get; }
        string Email { get; }
        string PhoneNumber { get; }
        string UserName { get; }
    }
}