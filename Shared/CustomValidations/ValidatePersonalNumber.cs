using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Project.Shared.CustomValidations
{
    public class ValidatePersonalNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) return new ValidationResult("Personal number is required");

            var personalNumber = value.ToString()!;
            if (!Regex.IsMatch(personalNumber, @"^\d{11}$"))
                return new ValidationResult("Personal number must contain exactly 11 digits");

            return ValidationResult.Success;
        }
    }
}
