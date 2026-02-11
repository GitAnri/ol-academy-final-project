using Project.Shared.Model;
using System.Linq.Expressions;

namespace Project.DAL.Repositories
{
    public interface IIndividualRepository
    {
        Task<Individual?> GetByIdAsync(int id);
        Task AddAsync(Individual individual);
        Task UpdateAsync(Individual individual);
        Task DeleteAsync(Individual individual);
        Task MarkIndividualDeletionAsync(Individual individual);
        Task<bool> ExistsByPersonalNumberAsync(string personalNumber, int? excludingId = null);
        Task<bool> CityExistsAsync(int cityId);
        Task<bool> IndividualExistsAsync(int individualId);
        Task<int> CountAsync(Expression<Func<Individual, bool>>? filter = null);
        Task<List<Individual>> GetAllAsync(Expression<Func<Individual, bool>>? filter = null, int skip = 0, int take = int.MaxValue);
    }
}
