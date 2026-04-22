using System.ComponentModel.DataAnnotations;
namespace onilne_service.Models
{
    public class BankAccountModel
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? BankName { get; set; }
        public string? AccountHolderName { get; set; }
        public string? IFSC { get; set; }
        public string? AccountNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
