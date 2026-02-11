using Microsoft.AspNetCore.Http;

namespace Business.Services
{
    public interface IFileService
    {
        Task<string> UploadIndividualImageAsync(int id, IFormFile file);
        Task DeleteIndividualImageAsync(string imagePath);
    }
}
