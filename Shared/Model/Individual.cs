namespace Project.Shared.Model
{
    public class Individual
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public Gender Gender { get; set; }
        public string PersonalNumber { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }

        public int CityId { get; set; }
        public City City { get; set; } = null!;

        public string? ImagePath { get; set; }

        public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
        public ICollection<Relation> Relations { get; set; } = new List<Relation>();
    }

    public enum Gender
    {
        Female,
        Male
    }
}
