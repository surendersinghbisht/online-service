using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using onilne_service.Entities;
using onilne_service.Model;
using onilne_service.Models;
using onilne_service.Service.Contract;
using System.Security.Claims;
using onilne_service.Models;

namespace onilne_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly UserManager<AspNetUser> _userManager;
        public BankController(IBankService bankService, UserManager<AspNetUser> userManager)
        {
            _bankService = bankService;
            _userManager = userManager;
        }

        //[HttpPost("add-bank-account")]
        //public async Task<IActionResult> AddBankAccount(BankAccount model)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return BadRequest();
        //    }

        //    model.UserId = userId;

        //    var res = await _bankService.AddBankAccount(model);
        //    if (!res.Status)
        //    {
        //        return BadRequest();
        //    }

        //    return Ok(res);
        //}

        [HttpPost("update-card")]
        public async Task<IActionResult> UpdateCardDetails(CardModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            model.UserId = userId;

            var res = await _bankService.AddCard(model);
            if (!res.Status)
            {
                return BadRequest();
            }

            return Ok(res);
        }

        [HttpPost("update-bank-account")]
        public async Task<IActionResult> UpdateBankDetails(BankAccountModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            model.UserId = userId;

            var res = await _bankService.AddBankAccount(model);
            if (!res.Status)
            {
                return BadRequest();
            }

            return Ok(res);
        }
    }
}
