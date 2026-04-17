using System.ComponentModel.DataAnnotations;

namespace onilne_service.DTOs
{
    public class ResetPassword
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Otp { get; set; }
    }
    public class SetNewPassword
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
