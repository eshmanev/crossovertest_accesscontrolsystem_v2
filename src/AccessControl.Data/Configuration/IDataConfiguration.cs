using System.Diagnostics.Contracts;
using AccessControl.Data.CodeContracts;

namespace AccessControl.Data.Configuration
{
    [ContractClass(typeof(IDataConfigurationContract))]
    public interface IDataConfiguration
    {
        bool RecreateDatabaseSchema { get; }
    }
}