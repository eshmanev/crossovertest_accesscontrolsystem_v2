using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    [ContractClass(typeof(UserExtendedContract))]
    public interface IUserExtended : IUser
    {
        string BiometricHash { get; }
    }
}