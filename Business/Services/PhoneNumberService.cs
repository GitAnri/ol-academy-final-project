using Business.Services;
using Project.DAL.Repositories;
using Project.Shared.Model;

namespace Project.Business.Services
{

    public class PhoneNumberService : IPhoneNumberService
    {
        private readonly IPhoneNumberRepository _repo;

        public PhoneNumberService(IPhoneNumberRepository repo) => _repo = repo;

        public async Task<bool> ExistsAsync(string number, int? excludingIndividualId = null) =>
            await _repo.ExistsAsync(number, excludingIndividualId);

        public async Task AddPhoneNumberAsync(PhoneNumber phoneNumber) =>
            await _repo.AddAsync(phoneNumber);

        public async Task UpdatePhoneNumberAsync(PhoneNumber phoneNumber) =>
            await _repo.UpdateAsync(phoneNumber);

        public async Task DeletePhoneNumberAsync(PhoneNumber phoneNumber) =>
            await _repo.DeleteAsync(phoneNumber);

        public async Task MarkDeletionAsync(PhoneNumber phoneNumber) =>
       await _repo.MarkDeletionAsync(phoneNumber);

        public async Task<List<PhoneNumber>> GetByIndividualIdAsync(int individualId) =>
            await _repo.GetByIndividualIdAsync(individualId);
    }
}
