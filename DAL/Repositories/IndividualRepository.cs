using Microsoft.EntityFrameworkCore;
using Project.DAL.Infrastructure;
using Project.Shared.Model;
using System.Linq.Expressions;

namespace Project.DAL.Repositories
{
    public class IndividualRepository : IIndividualRepository
    {
        private readonly IndividualsDbContext _context;
        public IndividualRepository(IndividualsDbContext context) => _context = context;

        public async Task AddAsync(Individual individual)
        {
            _context.Individuals.Add(individual);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Individual individual)
        {
            _context.Individuals.Remove(individual);
            await _context.SaveChangesAsync();
        }
        public async Task MarkIndividualDeletionAsync(Individual individual)
        {
            _context.Individuals.Remove(individual);
        }

        public async Task<List<Individual>> GetAllAsync(Expression<Func<Individual, bool>>? filter = null, int skip = 0, int take = int.MaxValue)
        {
            IQueryable<Individual> query = _context.Individuals
                .Include(i => i.City)
                .Include(i => i.PhoneNumbers)
                .Include(i => i.Relations)
                    .ThenInclude(r => r.RelatedIndividual);

            if (filter != null)
                query = query.Where(filter);

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<Individual, bool>>? filter = null)
        {
            IQueryable<Individual> query = _context.Individuals;
            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }

        public async Task<Individual?> GetByIdAsync(int id) =>
            await _context.Individuals
                .Include(i => i.City)
                .Include(i => i.PhoneNumbers)
                .Include(i => i.Relations)
                    .ThenInclude(r => r.RelatedIndividual)
                .FirstOrDefaultAsync(i => i.Id == id);

        public async Task UpdateAsync(Individual individual)
        {
            _context.Individuals.Update(individual);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByPersonalNumberAsync(string personalNumber, int? excludingId = null) =>
            await _context.Individuals
                .AnyAsync(i => i.PersonalNumber == personalNumber && (!excludingId.HasValue || i.Id != excludingId.Value));

        public async Task<bool> CityExistsAsync(int cityId) =>
            await _context.Cities.AnyAsync(c => c.Id == cityId);

        public async Task<bool> IndividualExistsAsync(int individualId) =>
            await _context.Individuals.AnyAsync(i => i.Id == individualId);
    }
}
