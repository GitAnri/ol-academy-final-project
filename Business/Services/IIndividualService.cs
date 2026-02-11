using Project.Shared.Command;         
using Shared.Query;

namespace Project.Business.Services
{
    public interface IIndividualService
    {
        Task<int> AddIndividualAsync(CreateIndividualCommandDto dto);
        Task UpdateIndividualAsync(int id, UpdateIndividualCommandDto dto);
        Task DeleteIndividualAsync(int id);
        Task<IndividualGetResponseDto?> GetByIdAsync(int id);
        Task<PagedResult<IndividualGetResponseDto>> GetAllAsync(IndividualQueryDto query);
    }
}
