using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUser" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUser))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IUserContract : IUser
    {
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

        public IUserGroup[] Groups
        {
            get
            {
                Contract.Ensures(Contract.Result<IUserGroup[]>() != null);
                return null;
            }
        }

        public bool IsManager => false;
        public string ManagerName => null;
        public string PhoneNumber => null;
        public string Department => null;
    }
}