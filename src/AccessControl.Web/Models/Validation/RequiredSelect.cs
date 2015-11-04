using System.ComponentModel.DataAnnotations;

namespace AccessControl.Web.Models.Validation
{
    public class RequiredSelect : ValidationAttribute
    {
        public const string Empty = "--";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var str = value as string;
            if (string.IsNullOrWhiteSpace(str) || Equals(str, Empty))
                return Error(validationContext);

            return null;
        }

        private ValidationResult Error(ValidationContext validationContext)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}