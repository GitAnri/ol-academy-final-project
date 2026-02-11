using Project.Shared.Model;

namespace Business.Services
{
    public interface IPhoneNumberService
    {
        Task<bool> ExistsAsync(string number, int? excludingIndividualId = null);
        Task AddPhoneNumberAsync(PhoneNumber phoneNumber);
        Task UpdatePhoneNumberAsync(PhoneNumber phoneNumber);
        Task DeletePhoneNumberAsync(PhoneNumber phoneNumber);
        Task MarkDeletionAsync(PhoneNumber phoneNumber);
        Task<List<PhoneNumber>> GetByIndividualIdAsync(int individualId);
    }
}
