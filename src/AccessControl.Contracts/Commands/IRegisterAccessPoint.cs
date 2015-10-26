using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(RegisterAccessPointContract))]
    public interface IRegisterAccessPoint
    {
       IAccessPoint AccessPoint { get; }
    }
}