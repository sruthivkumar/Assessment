using System.ComponentModel.DataAnnotations;

namespace ProductOrderAPI.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required]
        public string FullName { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MinLength(8)]
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User";
    }
}
