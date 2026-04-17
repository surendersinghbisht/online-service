using onilne_service.Entities;
using onilne_service.DTOs;
using onilne_service.Service.Contract;

namespace onilne_service.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDetail> GetUserDetails()
        {
            var user = _context.Users.FirstOrDefault();
            var bank = _context.BankCards.FirstOrDefault();

            return new UserDetail
            {
                Email = user?.Email ?? ""
            };
        }
    }
}
