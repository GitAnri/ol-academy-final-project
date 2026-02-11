using Project.Shared.CustomValidations;
using Project.Shared.Model;
using Shared.Command;
using System.ComponentModel.DataAnnotations;

namespace Project.Shared.Command
{
    public class CreateIndividualCommandDto
    {
        [Required]
        [ValidateName]
        public string FirstName { get; set; } = null!;

        [Required]
        [ValidateName]
        public string LastName { get; set; } = null!;

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [ValidatePersonalNumber]
        public string PersonalNumber { get; set; } = null!;

        [ValidateDateOfBirth]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public CityEnum City { get; set; }

        public List<CreatePhoneNumberDto> PhoneNumbers { get; set; } = new();
    }

    public class CreatePhoneNumberDto
    {
        [Required]
        [ValidatePhoneNumber]
        public string Number { get; set; } = null!;

        [Required]
        public PhoneNumberType Type { get; set; }
    }
}
