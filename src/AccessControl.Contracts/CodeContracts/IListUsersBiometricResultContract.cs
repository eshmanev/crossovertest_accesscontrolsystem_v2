using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListUsersBiometricResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListUsersBiometricResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListUsersBiometricResultContract : IListUsersBiometricResult
    {
        public IUserBiometric[] Users
        {
            get
            {
                Contract.Ensures(Contract.Result<IUserBiometric[]>() != null);
                return null;
            }
        }
    }
}