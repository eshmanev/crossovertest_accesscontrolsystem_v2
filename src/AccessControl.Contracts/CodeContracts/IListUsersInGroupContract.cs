using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Lists;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListUsersInGroup" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListUsersInGroup))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListUsersInGroupContract : IListUsersInGroup
    {
        public string UserGroupName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}