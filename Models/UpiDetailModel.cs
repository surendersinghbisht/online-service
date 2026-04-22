namespace onilne_service.Models
{
    public class UpiDetailModel
    {
        public string? Id { get; set; } = null!;

        public string? UserId { get; set; } = null!;

        public string UpiId { get; set; } = null!;

        public string AccountHolderName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }
    }
}
