using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class VoidResult : IVoidResult
    {
        public VoidResult()
        {
            Succeded = true;
        }

        public VoidResult(string errorSummary)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(errorSummary));
            Fault = new ValidationFault(errorSummary);
        }

        public VoidResult(string propertyName, string errorMessage)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(propertyName));
            Contract.Requires(!string.IsNullOrWhiteSpace(errorMessage));
            var fault = new ValidationFault();
            fault.AddError(propertyName, errorMessage);
            Fault = fault;
        }

        public VoidResult(IValidationFault fault)
        {
            Contract.Requires(fault != null);
            Fault = fault;
            Succeded = false;
        }

        public bool Succeded { get; }

        public IValidationFault Fault { get; }
    }
}