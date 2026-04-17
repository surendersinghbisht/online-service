using System.ComponentModel.DataAnnotations;
namespace onilne_service.Models
{
    public class BankAccount
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string BankName { get; set; }

        [Required]
        [MaxLength(20)]
        public string CardNumber { get; set; }

        [Required]
        [MaxLength(15)]
        public string MobileNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string CardHolderName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
