namespace Project.Shared.Model
{
    public class PhoneNumber
    {
        public int Id { get; set; }

        public string Number { get; set; } = null!;
        public PhoneNumberType Type { get; set; }

        public int IndividualId { get; set; }
        public Individual Individual { get; set; } = null!;
    }

    public enum PhoneNumberType
    {
        Mobile,
        Office,
        Home
    }
}
