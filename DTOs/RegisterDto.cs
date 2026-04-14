using System.ComponentModel.DataAnnotations;

namespace onilne_service.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        public string? Otp { get; set; } = string.Empty;
    }
}
