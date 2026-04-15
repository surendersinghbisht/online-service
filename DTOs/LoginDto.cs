namespace onilne_service.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? Otp { get; set; } = string.Empty;
    }
}
