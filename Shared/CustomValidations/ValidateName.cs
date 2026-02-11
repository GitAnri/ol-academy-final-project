using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Project.Shared.CustomValidations
{
    public class ValidateName : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) return new ValidationResult($"{context.DisplayName} is required");

            var name = value.ToString()!;
            if (name.Length < 2 || name.Length > 50)
                return new ValidationResult($"{context.DisplayName} must be between 2 and 50 characters");

            var georgian = new Regex(@"^[ა-ჰ]+$");
            var latin = new Regex(@"^[A-Za-z]+$");

            if (!(georgian.IsMatch(name) || latin.IsMatch(name)))
                return new ValidationResult($"{context.DisplayName} must contain only Georgian or only Latin letters");

            return ValidationResult.Success;
        }
    }
}
