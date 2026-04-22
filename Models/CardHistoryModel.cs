namespace onilne_service.Models
{
    public class CardHistoryModel
    {
        public string? Id { get; set; } = null!;

        public string BankAccountId { get; set; } = null!;

        public string? UserId { get; set; } = null!;

        public string BankName { get; set; } = null!;

        public string CardNumber { get; set; } = null!;

        public string MobileNumber { get; set; } = null!;

        public string CardHolderName { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public DateTime? ArchivedAt { get; set; }
    }
}
