using System.Security.Cryptography;
using System.Text;

namespace Shared.Helpers
{
    public static class PasswordHelper
    {
        public static (string Hash, string Salt) HashPassword(string password)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(16);
            var salt = Convert.ToBase64String(saltBytes);

            var combined = salt + password;

            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
            var hash = Convert.ToBase64String(hashBytes);

            return (hash, salt);
        }

        public static bool VerifyPassword(string password, string salt, string hash)
        {
            var combined = salt + password;
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
            var computedHash = Convert.ToBase64String(hashBytes);
            return hash == computedHash;
        }
    }

}
