using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUserDeleted" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUserDeleted))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IUserDeletedContract : IUserDeleted
    {
        public string UserName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}