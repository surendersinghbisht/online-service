using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using onilne_service.Models;
using onilne_service.Service.Contract;
using System.Security.Claims;

namespace onilne_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        public BankController(IBankService bankService)
        {
            _bankService = bankService;
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

        [HttpPost("update-bank-account")]
        public async Task<IActionResult> UpdateBankAccount(BankAccount model)
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
