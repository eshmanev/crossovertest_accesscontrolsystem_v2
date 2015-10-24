using System.Diagnostics.Contracts;

namespace Vendor.API.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAccessCheckService" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAccessCheckService))]
    // ReSharper disable once InconsistentNaming
    internal abstract class AccessCheckServiceContract : IAccessCheckService
    {
        public bool TryPass(AccessCheckDto dto)
        {
            Contract.Requires(dto != null);
            return false;
        }
    }
}