using System.ComponentModel.DataAnnotations;

namespace Project.Shared.CustomValidations
{
    public class ValidatePhoneNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) return new ValidationResult("Phone number is required");

            var number = value.ToString()!;
            if (number.Length < 4 || number.Length > 50)
                return new ValidationResult("Phone number must be between 4 and 50 characters");

            return ValidationResult.Success;
        }
    }
}
