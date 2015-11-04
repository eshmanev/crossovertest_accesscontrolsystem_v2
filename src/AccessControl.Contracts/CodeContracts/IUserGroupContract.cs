using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUserGroup" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUserGroup))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IUserGroupContract : IUserGroup
    {
        public string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}