

using System;

namespace AccessControl.Contracts.Dto
{
    using System.Diagnostics.Contracts;
    using AccessControl.Contracts.CodeContracts;

    [ContractClass(typeof(IAccessPointContract))]
    public interface IAccessPoint
    {
        Guid AccessPointId { get; }
        string Department { get; }
        string Site { get; }
        string Name { get; }
        string Description { get; }
    }
}