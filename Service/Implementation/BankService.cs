using onilne_service.Entities;
using onilne_service.DTOs;
using onilne_service.Models;
using onilne_service.Service.Contract;
using Microsoft.EntityFrameworkCore;

namespace onilne_service.Service.Implementation
{
    public class BankService : IBankService
    {
        private readonly AppDbContext _context;
        public BankService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseStatus> AddBankAccount(BankAccount model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (model == null)
                {
                    return new ResponseStatus
                    {
                        Status = false,
                        Message = "Invalid request"
                    };
                }

                var bankDetail = await _context.BankCards.FirstOrDefaultAsync(x => x.UserId == model.UserId);

                //is acc exists
                if (bankDetail == null)
                {
                    var newAccount = new BankCard
                    {
                        Id = Guid.NewGuid().ToString(),
                        BankName = model.BankName,
                        CardHolderName = model.CardHolderName,
                        CardNumber = model.CardNumber,
                        MobileNumber = model.MobileNumber,
                        UserId = model.UserId,
                    };

                    await _context.BankCards.AddAsync(newAccount);
                }
                else
                {
                    var oldAccount = new BankAccountHistory
                    {
                        Id = Guid.NewGuid().ToString(),
                        BankAccountId = bankDetail.Id,
                        BankName = bankDetail.BankName,
                        CardHolderName = bankDetail.CardHolderName,
                        CardNumber = bankDetail.CardNumber,
                        MobileNumber = bankDetail.MobileNumber,
                        UserId = bankDetail.UserId,
                        ArchivedAt = DateTime.UtcNow
                    };

                    await _context.BankAccountHistories.AddAsync(oldAccount);

                    _context.BankCards.Remove(bankDetail);

                    var updatedAccount = new BankCard
                    {
                        Id = Guid.NewGuid().ToString(),
                        BankName = model.BankName,
                        CardHolderName = model.CardHolderName,
                        CardNumber = model.CardNumber,
                        MobileNumber = model.MobileNumber,
                        UserId = model.UserId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.BankCards.AddAsync(updatedAccount);
                }


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseStatus
                {
                    Status = true,
                    Message = bankDetail == null ? "Bank account added successfully." : "Bank account updated successfully."
                };

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResponseStatus
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseData<UserDetail>> GetBankUserDetail(string userId)
        {
            var response = new ResponseData<UserDetail>();
            try
            {
                var user =  _context.Users.FirstOrDefault(x => x.Id == userId);
                var detail = await _context.BankCards.FirstOrDefaultAsync(x => x.UserId == userId);

                if (user == null || detail == null)
                {
                    response.Status = false;
                    response.Message = "Not Found";
                    return response;
                }

                var bankDetail = new BankAccount
                {
                    BankName = detail.BankName,
                    CardHolderName = detail.CardHolderName,
                    CardNumber = detail.CardNumber,
                    CreatedAt = detail.CreatedAt.Value,
                    MobileNumber = detail.MobileNumber,
                    UserId = detail.UserId,
                    Id = detail.Id
                };

                var data = new UserDetail
                {
                    Email = user.Email,
                    Name = user.Name,
                    UserID = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    Bank = bankDetail
                };

                response.Data = data;
                response.Status = true;
                response.Message = "Successfully fetched details";
                return response;

            }
            catch (Exception ex)
            {
                return response;
            }
        }
    }
}
