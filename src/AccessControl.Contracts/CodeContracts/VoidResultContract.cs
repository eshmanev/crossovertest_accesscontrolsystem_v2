using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IVoidResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IVoidResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class VoidResultContract : IVoidResult
    {
        public bool Succeded => false;

        public IValidationFault Fault => null;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Requires(Succeded || Fault != null);
        }
    }
}