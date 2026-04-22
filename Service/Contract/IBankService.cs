using onilne_service.DTOs;
using onilne_service.Models;

namespace onilne_service.Service.Contract
{
    public interface IBankService
    {
        Task<ResponseStatus> AddCard(CardModel model);
        Task<ResponseData<UserDetail>> GetUserDetail(string userId);
        Task<ResponseStatus> AddBankAccount(BankAccountModel model);
        Task<ResponseStatus> AddUpiDetail(UpiDetailModel model);
    }
}
