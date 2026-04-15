using Microsoft.AspNetCore.Identity;

namespace onilne_service.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string NormalizedCustomEmail { get; set; } = string.Empty;
    }
}
