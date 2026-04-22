using Microsoft.EntityFrameworkCore;
using onilne_service.DTOs;
using onilne_service.Entities;
using onilne_service.Model;
using onilne_service.Models;
using onilne_service.Service.Contract;

namespace onilne_service.Service.Implementation
{
    public class BankService : IBankService
    {
        private readonly AppDbContext _context;
        public BankService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseStatus> AddCard(CardModel model)
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

                var cardDetail = await _context.Cards.FirstOrDefaultAsync(x => x.UserId == model.UserId);

                //if card not exists
                if (cardDetail == null)
                {
                    var newCard = new Card
                    {
                        Id = Guid.NewGuid().ToString(),
                        CardHolderName = model.CardHolderName,
                        CardNumber = model.CardNumber,
                        MobileNumber = model.MobileNumber,
                        UserId = model.UserId,
                    };

                    await _context.Cards.AddAsync(newCard);
                }
                else
                {
                    var oldcard = new CardsHistory
                    {
                        Id = Guid.NewGuid().ToString(),
                        BankAccountId = cardDetail.Id,
                        BankName = cardDetail.BankName,
                        CardHolderName = cardDetail.CardHolderName,
                        CardNumber = cardDetail.CardNumber,
                        MobileNumber = cardDetail.MobileNumber,
                        UserId = cardDetail.UserId,
                        ArchivedAt = DateTime.UtcNow
                    };

                    await _context.CardsHistories.AddAsync(oldcard);

                    _context.Cards.Remove(cardDetail);

                    var updatedCard = new Card
                    {
                        Id = Guid.NewGuid().ToString(),
                        CardHolderName = model.CardHolderName,
                        CardNumber = model.CardNumber,
                        MobileNumber = model.MobileNumber,
                        UserId = model.UserId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.Cards.AddAsync(updatedCard);
                }


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseStatus
                {
                    Status = true,
                    Message = cardDetail == null ? "Card added successfully." : "Card updated successfully."
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
        public async Task<ResponseStatus> AddBankAccount(BankAccountModel model)
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

                var bankDetail = await _context.BankAccounts.FirstOrDefaultAsync(x => x.UserId == model.UserId);

                //if card not exists
                if (bankDetail == null)
                {
                    var newBankAccount = new BankAccount
                    {
                        Id = Guid.NewGuid().ToString(),
                        BankName = model.BankName,
                        AccountHolderName = model.AccountHolderName,
                        AccountNumber = model.AccountNumber,
                        Ifsccode = model.IFSC,
                        UserId = model.UserId,
                    };

                    await _context.BankAccounts.AddAsync(newBankAccount);
                }
                else
                {
                    var oldBankAccount = new BankAccountHistory
                    {
                        Id = Guid.NewGuid().ToString(),
                        AccountHolderName = bankDetail.AccountHolderName,
                        AccountNumber = bankDetail.AccountNumber,
                        BankAccountId = bankDetail.Id,
                        Ifsccode = bankDetail.Ifsccode,
                        UserId = bankDetail.UserId,
                        BankName = bankDetail.BankName,
                        ArchivedAt = DateTime.UtcNow
                    };

                    await _context.BankAccountHistories.AddAsync(oldBankAccount);

                    _context.BankAccounts.Remove(bankDetail);

                    var updatedBankAccount = new BankAccount
                    {
                        Id = Guid.NewGuid().ToString(),
                        AccountNumber = model.AccountNumber,
                        AccountHolderName = model.AccountHolderName,
                        BankName = model.BankName,
                        Ifsccode = model.IFSC,
                        UserId = model.UserId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.BankAccounts.AddAsync(updatedBankAccount);
                }


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseStatus
                {
                    Status = true,
                    Message = bankDetail == null ? "Bank Account added successfully." : "Bank Account updated successfully."
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

        public async Task<ResponseData<UserDetail>> GetUserDetail(string userId)
        {
            var response = new ResponseData<UserDetail>();
            try
            {
                var user = await _context.AspNetUsers
     .FirstOrDefaultAsync(x => x.Id == userId);

                var card = await _context.Cards
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                var bank = await _context.BankAccounts
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                var result = new
                {
                    User = user,
                    Card = card,
                    BankAccount = bank
                };

                if (result == null)
                {
                    response.Status = false;
                    response.Message = "Not Found";
                    return response;
                }

                var CardDetail = new CardModel
                {
                    CardHolderName = result.Card.CardHolderName,
                    CardNumber = result.Card.CardNumber,
                    CreatedAt = result.Card.CreatedAt.Value,
                    MobileNumber = result.Card.MobileNumber,
                    UserId = result.Card.UserId,
                    Id = result.Card.Id
                };

                var bankAccountDetail = new BankAccountModel
                {
                    AccountHolderName = result.BankAccount.AccountHolderName,
                    CreatedAt = result.BankAccount.CreatedAt.Value,
                    AccountNumber = result.BankAccount.AccountNumber,
                    BankName = result.BankAccount.BankName,
                    IFSC = result.BankAccount.Ifsccode,
                    UserId = result.BankAccount.UserId,
                    Id = result.BankAccount.Id
                };

                var data = new UserDetail
                {
                    Email = result.User.Email,
                    Name = result.User.Name,
                    UserID = result.User.Id,
                    PhoneNumber = result.User.PhoneNumber,
                    Card = CardDetail,
                    BankAccount = bankAccountDetail
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
