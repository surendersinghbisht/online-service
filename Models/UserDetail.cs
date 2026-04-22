using onilne_service.Models;

namespace onilne_service.DTOs
{
    public class UserDetail
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public CardModel Card { get; set; }
        public BankAccountModel BankAccount { get; set; }

    }
}
