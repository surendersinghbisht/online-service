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
                        BankName = model.BankName,
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
                        BankName = model.BankName,
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
        public async Task<ResponseStatus> AddUpiDetail(UpiDetailModel model)
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

                var upiDetail = await _context.Upis.FirstOrDefaultAsync(x => x.UserId == model.UserId);

                //if upi not exists
                if (upiDetail == null)
                {
                    var newUpi = new Upi
                    {
                        Id = Guid.NewGuid().ToString(),
                        AccountHolderName = model.AccountHolderName,
                        PhoneNumber = model.PhoneNumber,
                        UpiId = model.UpiId,
                        UserId = model.UserId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.Upis.AddAsync(newUpi);
                }
                else
                {
                    var oldUpi = new UpiHistory
                    {
                        Id = Guid.NewGuid().ToString(),
                        AccountHolderName = upiDetail.AccountHolderName,
                        PhoneNumber = upiDetail.PhoneNumber,
                        UpiId = upiDetail.UpiId,
                        UserId = upiDetail.UserId,
                        CreatedAt = upiDetail.CreatedAt,
                        ArchivedAt = DateTime.UtcNow
                    };

                    await _context.UpiHistories.AddAsync(oldUpi);

                    _context.Upis.Remove(upiDetail);

                    var updatedUpi = new Upi
                    {
                        Id = Guid.NewGuid().ToString(),
                        PhoneNumber = model.PhoneNumber,
                        AccountHolderName = model.AccountHolderName,
                        UpiId = model.UpiId,
                        UserId = model.UserId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.Upis.AddAsync(updatedUpi);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseStatus
                {
                    Status = true,
                    Message = upiDetail == null ? "UPI added successfully." : "UPI updated successfully."
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

                if (user == null)
                {
                    response.Status = false;
                    response.Message = "User Not Found";
                    return response;
                }

                var card = await _context.Cards
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                var bank = await _context.BankAccounts
                    .FirstOrDefaultAsync(x => x.UserId == userId);
                var upi = await _context.Upis
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                CardModel? cardDetail = null;

                if (card != null)
                {
                    cardDetail = new CardModel
                    {
                        Id = card.Id,
                        CardHolderName = card.CardHolderName,
                        CardNumber = card.CardNumber,
                        MobileNumber = card.MobileNumber,
                        BankName = card.BankName,
                        UserId = card.UserId,
                        CreatedAt = card.CreatedAt
                    };
                }

                BankAccountModel? bankDetail = null;

                if (bank != null)
                {
                    bankDetail = new BankAccountModel
                    {
                        Id = bank.Id,
                        AccountHolderName = bank.AccountHolderName,
                        AccountNumber = bank.AccountNumber,
                        BankName = bank.BankName,
                        IFSC = bank.Ifsccode,
                        UserId = bank.UserId,
                        CreatedAt = bank.CreatedAt.Value
                    };
                }

                UpiDetailModel? upiDetail = null;

                if (upi != null)
                {
                    upiDetail = new UpiDetailModel
                    {
                        Id = bank.Id,
                        AccountHolderName = upi.AccountHolderName,
                        UpiId = upi.UpiId,
                        PhoneNumber = upi.PhoneNumber,
                        UserId = bank.UserId,
                        CreatedAt = bank.CreatedAt.Value
                    };
                }

                var data = new UserDetail
                {
                    Email = user.Email,
                    Name = user.Name,
                    UserID = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    Card = cardDetail,
                    BankAccount = bankDetail,
                    Upi = upiDetail,
                };

                response.Data = data;
                response.Status = true;
                response.Message = "Successfully fetched details";

                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
