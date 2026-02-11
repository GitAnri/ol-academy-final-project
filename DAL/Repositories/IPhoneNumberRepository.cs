using Project.Shared.Model;

namespace Project.DAL.Repositories
{
    public interface IPhoneNumberRepository
    {
        Task<bool> ExistsAsync(string number, int? excludingIndividualId = null);
        Task AddAsync(PhoneNumber phoneNumber);
        Task UpdateAsync(PhoneNumber phoneNumber);
        Task DeleteAsync(PhoneNumber phoneNumber);
        Task MarkDeletionAsync(PhoneNumber phoneNumber);
        Task<List<PhoneNumber>> GetByIndividualIdAsync(int individualId);
    }
}