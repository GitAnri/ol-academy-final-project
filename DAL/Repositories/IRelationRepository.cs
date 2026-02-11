using Project.Shared.Model;

namespace Project.DAL.Repositories
{
    public interface IRelationRepository
    {
        Task<bool> ExistsAsync(int individualId, int relatedId, RelationType type);
        Task AddAsync(Relation relation);
        Task DeleteAsync(Relation relation);
        Task MarkDeletionByIndividualIdAsync(int individualId);
        Task<Relation?> GetAsync(int individualId, int relationId);
        Task<List<Relation>> GetByIndividualIdAsync(int individualId);
        Task DeleteByIndividualIdAsync(int individualId);
        Task<List<Individual>> GetAllIndividualsWithRelationsAsync();
    }
}
