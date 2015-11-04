using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    [ContractClass(typeof(IAccessPointContract))]
    public interface IAccessPoint
    {
        Guid AccessPointId { get; }
        string Department { get; }
        string Description { get; }
        string Name { get; }
        string Site { get; }
    }
}