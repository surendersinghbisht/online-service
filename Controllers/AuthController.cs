using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using onilne_service.Contract;
using onilne_service.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace onilne_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration, IMemoryCache cache, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _cache = cache;
            _emailService = emailService;
        }

        [HttpPost("send-otp")]
        public IActionResult SendOtp(string email)
        {
            if (_cache.TryGetValue(email, out _))
            {
                return BadRequest("OTP already sent. Please wait.");
            }

            var otp = new Random().Next(100000, 999999).ToString();

            _cache.Set(email, otp, TimeSpan.FromMinutes(5));

            _cache.Set($"{email}_cooldown", true, TimeSpan.FromSeconds(60));

            Console.WriteLine($"OTP for {email}: {otp}");

            return Ok("OTP sent successfully");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userName = model.Email;
            var userExists = await _userManager.FindByNameAsync(model.Email);
            if (userExists != null)
                return BadRequest("User already exists!");

            if (_cache.TryGetValue(model.Email, out _))
                return BadRequest("OTP already sent. Please wait.");

            var otp = new Random().Next(100000, 999999).ToString();

            _cache.Set(model.Email, otp, TimeSpan.FromMinutes(5));

            await _emailService.SendOtpAsync(model.Email, otp);
            Console.WriteLine($"OTP for {model.Email}: {otp}");

            return Ok(new
            {
                success = true,
                message = "OTP sent successfully"
            });

        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(RegisterDto model) {
            var storedOtp = _cache.Get<string>(model.Email);
            if (storedOtp == null)
                return BadRequest("OTP expired");

            if (storedOtp != model.Otp)
                return BadRequest("Invalid OTP");

            _cache.Remove(model.Email);

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token = token,
                status = true,
                message = "Registration successful"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {

                var token = GenerateJwtToken(user);

                return Ok(new AuthResponseDto
                {
                    Token = token,
                });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email)
    };

            var key = new SymmetricSecurityKey(
       Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
   );

            var token = new JwtSecurityToken(
                issuer: "yourapp",
                audience: "yourapp",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
