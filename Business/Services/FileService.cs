using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Project.DAL.Repositories;
using Microsoft.Extensions.Hosting;
using Project.Business.Exceptions;

namespace Business.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepo;
        private readonly IIndividualRepository _individualRepo;
        private readonly IHostEnvironment _env;

        public FileService(
            IFileRepository fileRepo,
            IIndividualRepository individualRepo,
            IHostEnvironment env)
        {
            _fileRepo = fileRepo;
            _individualRepo = individualRepo;
            _env = env;
        }

        public async Task DeleteIndividualImageAsync(string imagePath)
        {
            var fullPath = Path.Combine(_env.ContentRootPath, "Uploads", imagePath.TrimStart('/'));
            await _fileRepo.DeleteAsync(fullPath);
        }

        public async Task<string> UploadIndividualImageAsync(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ValidationException("File is required");

            var individual = await _individualRepo.GetByIdAsync(id);
            if (individual == null)
                throw new NotFoundException("Individual not found");

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "Uploads", "images");

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"individual_{id}_{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            if (!string.IsNullOrEmpty(individual.ImagePath))
            {
                var oldFullPath = Path.Combine(
                    _env.ContentRootPath,
                    "Uploads",
                    individual.ImagePath.TrimStart('/'));

                await _fileRepo.DeleteAsync(oldFullPath);
            }

            await _fileRepo.SaveAsync(file.OpenReadStream(), fullPath);

            individual.ImagePath = $"/images/{fileName}";
            await _individualRepo.UpdateAsync(individual);

            return individual.ImagePath;
        }
    }
}
