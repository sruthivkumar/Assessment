using System.ComponentModel.DataAnnotations;

namespace ProductOrderAPI.Model
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string FullName { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string PasswordHash { get; set; } = null!;
        [Required]
        public string Role { get; set; } = "User"; // Admin/User
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
