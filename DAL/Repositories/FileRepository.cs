namespace DAL.Repositories
{
    public class FileRepository : IFileRepository
    {
        public async Task<string> SaveAsync(Stream stream, string fullPath)
        {
            var dir = Path.GetDirectoryName(fullPath)!;
            Directory.CreateDirectory(dir);

            using var fs = new FileStream(fullPath, FileMode.Create);
            await stream.CopyToAsync(fs);
            return fullPath;
        }

        public Task DeleteAsync(string fullPath)
        {
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }
    }


}
