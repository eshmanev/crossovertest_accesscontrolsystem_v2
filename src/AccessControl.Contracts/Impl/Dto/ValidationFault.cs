using System.Collections.Generic;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class ValidationFault : IValidationFault
    {
        private readonly List<IValidationPropertyError> _detailsInternal;

        public ValidationFault(string summary = "Faulted validation.")
        {
            Summary = summary;
            _detailsInternal = new List<IValidationPropertyError>();
        }

        public string Summary { get; }

        public IValidationPropertyError[] Details => _detailsInternal.ToArray();

        public void AddError(string propertyName, string message)
        {
            _detailsInternal.Add(new PropertyError(message, propertyName));
        }

        private class PropertyError : IValidationPropertyError
        {
            public PropertyError(string message, string propertyName)
            {
                Message = message;
                PropertyName = propertyName;
            }

            public string Message { get; }
            public string PropertyName { get; }
        }
    }
}