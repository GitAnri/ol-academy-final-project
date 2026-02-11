namespace DAL.Repositories
{
    public interface IFileRepository
    {
        Task<string> SaveAsync(Stream fileStream, string fileName);
        Task DeleteAsync(string filePath);
    }

}
