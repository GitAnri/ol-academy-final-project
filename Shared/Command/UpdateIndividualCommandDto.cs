using Project.Shared.CustomValidations;
using Project.Shared.Model;
using Shared.Command;
using System.ComponentModel.DataAnnotations;

namespace Project.Shared.Command
{
    public class UpdateIndividualCommandDto
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

        [Required]
        public List<UpdatePhoneNumberDto> PhoneNumbers { get; set; } = new();
    }

    public class UpdatePhoneNumberDto
    {
        [Required]
        [ValidatePhoneNumber]
        public string Number { get; set; } = null!;

        [Required]
        public PhoneNumberType Type { get; set; }
    }
}
