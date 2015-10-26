using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(IListAccessPointsResultContract))]
    public interface IListAccessPointsResult
    {
        IAccessPoint[] AccessPoints { get; }
    }
}