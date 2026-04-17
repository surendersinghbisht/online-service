using onilne_service.DTOs;
using onilne_service.Models;

namespace onilne_service.Service.Contract
{
    public interface IBankService
    {
        Task<ResponseStatus> AddBankAccount(BankAccount model);
        Task<ResponseData<UserDetail>> GetBankUserDetail(string userId);
    }
}
