using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListUsersExtended" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListUsersExtended))]
    // ReSharper disable once InconsistentNaming
    internal abstract class ListUsersExtendedContract : IListUsersExtended
    {
        public string Site
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public string Department
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}