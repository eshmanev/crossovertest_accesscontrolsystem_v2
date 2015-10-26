using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListBiometricInfo" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListBiometricInfo))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListBiometricInfoContract : IListBiometricInfo
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