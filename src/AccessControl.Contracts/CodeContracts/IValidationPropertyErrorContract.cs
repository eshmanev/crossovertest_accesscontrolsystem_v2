using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IValidationPropertyError" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IValidationPropertyError))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IValidationPropertyErrorContract : IValidationPropertyError
    {
        public string PropertyName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public string Message
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}