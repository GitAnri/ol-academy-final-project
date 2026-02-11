using DAL.Models;
using Project.Shared.Model;

namespace Business.Services
{
    public interface IUserService
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> RegisterAsync(string username, string password);
    }
}
