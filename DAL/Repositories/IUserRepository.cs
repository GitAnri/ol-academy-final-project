using DAL.Models;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int id);
    }
}
