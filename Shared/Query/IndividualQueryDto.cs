using Project.Shared.Model;

namespace Shared.Query
{
    public class IndividualQueryDto
    {
        public string? Search { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PersonalNumber { get; set; }
        public Gender? Gender { get; set; }
        public int? CityId { get; set; }
        public DateTime? DateOfBirthFrom { get; set; }
        public DateTime? DateOfBirthTo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
