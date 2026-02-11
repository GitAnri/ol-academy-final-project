namespace Project.Shared.Model
{
    public class Relation
    {
        public int Id { get; set; }

        public int IndividualId { get; set; }
        public Individual Individual { get; set; } = null!;

        public int RelatedIndividualId { get; set; }
        public Individual RelatedIndividual { get; set; } = null!;

        public RelationType RelationType { get; set; }
    }

    public enum RelationType
    {
        Colleague,
        Acquaintance,
        Relative,
        Other
    }
}
