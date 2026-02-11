using Project.Shared.Model;

namespace Shared.Query
{
    public class IndividualGetResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public Gender Gender { get; set; }
        public string PersonalNumber { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public IndividualCityDto City { get; set; } = null!;
        public List<PhoneNumberDto> PhoneNumbers { get; set; } = new();
        public string? ImagePath { get; set; }
        public List<RelationDto> Relations { get; set; } = new();
    }

    public class PhoneNumberDto
    {
        public string Number { get; set; } = null!;
        public PhoneNumberType Type { get; set; }
    }

    public class RelationDto
    {
        public int RelationId { get; set; }
        public int RelatedIndividualId { get; set; }
        public RelationType RelationType { get; set; }
    }
}
