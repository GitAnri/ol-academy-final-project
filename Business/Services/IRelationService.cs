using Project.Shared.Model;
using Shared.Query;

namespace Business.Services
{
    public interface IRelationService
    {
        Task AddRelationAsync(Relation relation);
        Task DeleteRelationAsync(Relation relation);
        Task<Relation?> GetRelationAsync(int individualId, int relationId);
        Task<List<Relation>> GetByIndividualIdAsync(int individualId);
        Task DeleteByIndividualIdAsync(int individualId);
        Task MarkDeletionByIndividualIdAsync(int individualId);
        Task<bool> ExistsAsync(int individualId, int relatedId, RelationType type);
        Task<List<IndividualRelationsReportDto>> GetRelationsReportAsync();
    }
}
