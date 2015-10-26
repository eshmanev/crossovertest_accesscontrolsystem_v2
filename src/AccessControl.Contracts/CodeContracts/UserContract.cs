using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUser" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUser))]
    // ReSharper disable once InconsistentNaming
    internal abstract class UserContract : IUser
    {
        public string Site
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public string UserName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public string DisplayName => null;
        public string Email => null;
        public string PhoneNumber => null;
        public string Department => null;
    }
}