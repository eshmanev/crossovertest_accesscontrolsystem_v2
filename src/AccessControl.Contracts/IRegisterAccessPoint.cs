using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts
{
    [ContractClass(typeof(RegisterAccessPointContract))]
    public interface IRegisterAccessPoint
    {
        Guid AccessPointId { get; }
        string Description { get; }
        string Name { get; }
    }
}