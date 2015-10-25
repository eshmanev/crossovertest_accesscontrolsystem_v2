using System.Diagnostics.Contracts;
using AccessControl.Data.Configuration;

namespace AccessControl.Data.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IDataConfiguration" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IDataConfiguration))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IDataConfigurationContract : IDataConfiguration
    {
        public bool RecreateDatabaseSchema => false;
    }
}