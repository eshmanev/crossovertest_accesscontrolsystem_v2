using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts
{
    [ContractClass(typeof(IAccessPointContract))]
    public interface IAccessPoint
    {
        Guid AccessPointId { get; }
        string Description { get; }
        string Name { get; }
    }
}