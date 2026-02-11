using Shared.Helpers;
using DAL.Models;
using DAL.Repositories;
using Microsoft.Extensions.Configuration;


namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecret;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtSecret = configuration["JwtSettings:Secret"];
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !PasswordHelper.VerifyPassword(password, user.Salt, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password");

            return user;
        }

        public async Task<User> RegisterAsync(string username, string password)
        {
            if (await _userRepository.ExistsByUsernameAsync(username))
                throw new InvalidOperationException("Username already exists");

            var (hash, salt) = PasswordHelper.HashPassword(password);

            var user = new User
            {
                Username = username,
                PasswordHash = hash,
                Salt = salt,
                Role = "User"
            };

            await _userRepository.AddAsync(user);
            return user;
        }


    }
}
