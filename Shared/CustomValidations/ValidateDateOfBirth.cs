using System.ComponentModel.DataAnnotations;

namespace Project.Shared.CustomValidations
{
    public class ValidateDateOfBirth : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) return new ValidationResult("Date of birth is required");

            var date = (DateTime)value;
            if (date.AddYears(18) > DateTime.UtcNow)
                return new ValidationResult("Individual must be at least 18 years old");

            return ValidationResult.Success;
        }
    }
}
