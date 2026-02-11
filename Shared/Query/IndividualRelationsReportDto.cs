using Project.Shared.Model;

namespace Shared.Query
{
    public class IndividualRelationsReportDto
    {
        public int IndividualId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public Dictionary<RelationType, int> RelationsCount { get; set; } = new();
    }
}

