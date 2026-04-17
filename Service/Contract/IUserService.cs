using onilne_service.DTOs;

namespace onilne_service.Service.Contract
{
    public interface IUserService
    {
        Task<UserDetail> GetUserDetails();
    }
}
