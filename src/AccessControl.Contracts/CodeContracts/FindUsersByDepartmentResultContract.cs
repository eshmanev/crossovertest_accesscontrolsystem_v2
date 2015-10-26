using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IFindUsersByDepartmentResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IFindUsersByDepartmentResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class FindUsersByDepartmentResultContract : IFindUsersByDepartmentResult
    {
        public IUser[] Users
        {
            get
            {
                Contract.Ensures(Contract.Result<IUser[]>() != null);
                return null;
            }
        }
    }
}