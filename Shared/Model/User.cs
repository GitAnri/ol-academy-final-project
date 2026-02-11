using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public string Salt { get; set; } = null!;

        [Required]
        public string Role { get; set; } = "User"; 
    }
}
