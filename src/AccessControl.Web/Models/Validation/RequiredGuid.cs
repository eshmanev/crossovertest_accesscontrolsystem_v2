using System;
using System.ComponentModel.DataAnnotations;

namespace AccessControl.Web.Models.Validation
{
    public class RequiredGuid : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return Error(validationContext);

            Guid guid;
            if (value is string)
            {
                var str = (string)value;
                if (string.IsNullOrWhiteSpace(str))
                    return Error(validationContext);

                if (!Guid.TryParse(str, out guid))
                    return Error(validationContext);
            }
           else if (value is Guid)
           {
               guid = (Guid) value;
           }
           else
           {
               return Error(validationContext);
           }

            return Equals(guid, Guid.Empty) ? Error(validationContext) : null;
        }

        private ValidationResult Error(ValidationContext validationContext)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}