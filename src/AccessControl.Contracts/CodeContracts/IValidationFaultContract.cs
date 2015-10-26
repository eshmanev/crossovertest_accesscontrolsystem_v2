using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IValidationFault" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IValidationFault))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IValidationFaultContract : IValidationFault
    {
        public IValidationPropertyError[] Details
        {
            get
            {
                Contract.Ensures(Contract.Result<IValidationPropertyError[]>() != null);
                return null;
            }
        }

        public string Summary
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}