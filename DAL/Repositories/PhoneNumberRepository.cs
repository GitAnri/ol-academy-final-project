using Microsoft.EntityFrameworkCore;
using Project.DAL.Infrastructure;
using Project.Shared.Model;

namespace Project.DAL.Repositories
{
    public class PhoneNumberRepository : IPhoneNumberRepository
    {
        private readonly IndividualsDbContext _context;

        public PhoneNumberRepository(IndividualsDbContext context) => _context = context;

        public async Task<bool> ExistsAsync(string number, int? excludingIndividualId = null)
        {
            return await _context.PhoneNumbers
                .AnyAsync(p => p.Number == number &&
                               (!excludingIndividualId.HasValue || p.IndividualId != excludingIndividualId.Value));
        }

        public async Task AddAsync(PhoneNumber phoneNumber)
        {
            _context.PhoneNumbers.Add(phoneNumber);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhoneNumber phoneNumber)
        {
            _context.PhoneNumbers.Update(phoneNumber);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PhoneNumber phoneNumber)
        {
            _context.PhoneNumbers.Remove(phoneNumber);
            await _context.SaveChangesAsync();
        }
        public async Task MarkDeletionAsync(PhoneNumber phoneNumber)
        {
            _context.PhoneNumbers.Remove(phoneNumber);
        }

        public async Task<List<PhoneNumber>> GetByIndividualIdAsync(int individualId)
        {
            return await _context.PhoneNumbers
                .Where(p => p.IndividualId == individualId)
                .ToListAsync();
        }
    }
}
