using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using onilne_service.Contract;
using onilne_service.DTOs;
using onilne_service.Model;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace onilne_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMemoryCache cache, IEmailService emailService, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _cache = cache;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("send-otp")]
        public IActionResult SendOtp(string email)
        {
            if (_cache.TryGetValue(email, out _))
            {
                return BadRequest("OTP already sent. Please wait.");
            }

            var otp = new Random().Next(100000, 999999).ToString();

            _cache.Set(email, otp, TimeSpan.FromMinutes(2));

            _cache.Set($"{email}_cooldown", true, TimeSpan.FromSeconds(60));

            Console.WriteLine($"OTP for {email}: {otp}");

            return Ok("OTP sent successfully");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model for {Email}", model?.Email);
                    return BadRequest(ModelState);
                }

                var userExists = await _userManager.FindByNameAsync(model.Email);
                if (userExists != null)
                {
                    _logger.LogInformation("User already exists: {Email}", model.Email);
                    return BadRequest("User already exists!");
                }

                var normalizedEmail = NormalizeEmail(model.Email);

                var existingUser = await _userManager.Users
                    .FirstOrDefaultAsync(x => x.NormalizedCustomEmail == normalizedEmail);

                if (existingUser != null)
                {
                    _logger.LogInformation("Email already exists: {Email}", model.Email);
                    return BadRequest("Email already exists!");
                }


                var userWithPhone = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
                if (userWithPhone != null)
                {
                    _logger.LogInformation("Phone Number already exists: {PhoneNumber}", model.PhoneNumber);
                    return BadRequest("Phone Number already exists!");
                }

                if (_cache.TryGetValue(model.Email, out _))
                {
                    _logger.LogInformation("OTP already sent for {Email}", model.Email);
                    return BadRequest("OTP already sent. Please wait.");
                }

                var otp = new Random().Next(100000, 999999).ToString();
                _cache.Set(model.Email, otp, TimeSpan.FromMinutes(2));

                await _emailService.SendOtpAsync(model.Email, otp);

                _logger.LogInformation("OTP sent to {Email}", model.Email);

                return Ok(new { success = true, message = "OTP sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Register for {Email}", model?.Email);
                return StatusCode(500, "Something went wrong");
            }
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtp model)
        {
            var storedOtp = _cache.Get<string>(model.Email);
            if (storedOtp == null)
                return BadRequest("OTP expired");

            if (storedOtp != model.Otp)
                return BadRequest("Invalid OTP");

            _cache.Remove(model.Email);
            var normalizedEmail = NormalizeEmail(model.Email);

            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedCustomEmail == normalizedEmail);

            if (existingUser != null)
            {
                var token = GenerateJwtToken(existingUser);
                return Ok(new
                {
                    token,
                    status = true,
                    message = "Login successful",
                    name = $"{existingUser.Name}",
                    email = existingUser.Email
                });
            }
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                NormalizedCustomEmail = normalizedEmail,
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var newToken = GenerateJwtToken(user);

            return Ok(new
            {
                token = newToken,
                status = true,
                message = "Registration successful"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var normalizedEmail = NormalizeEmail(model.Email);

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.NormalizedCustomEmail == normalizedEmail);

            if(user == null)
            {
               return BadRequest(new ResponseStatus
                {
                    Status = false,
                    Message = "Invalid credentials."
               });
            }

            //normal login
            if (user != null && !string.IsNullOrEmpty(model.Password))
            {

                var isPasswordVaild = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!isPasswordVaild)
                {
                    return BadRequest(new ResponseStatus
                    {
                        Status = false,
                        Message = "Invalid credentials"
                    });
                }

                var token = GenerateJwtToken(user);

                return Ok(new
                {
                    Token = token,
                    status = true,
                    Message = "login sucessfull",
                    Name = $"{user.Name}",
                    email = $"{user.Email}"
                });
            }

            //otp login
                var otp = new Random().Next(100000, 999999).ToString();
                _cache.Set(model.Email, otp, TimeSpan.FromMinutes(2));

                await _emailService.SendOtpAsync(model.Email, otp);

                return Ok(new ResponseStatus
                {
                    Status = true,
                    Message = "Otp sent successfully"
                });
        }

        [HttpGet("forget-password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(
                    new ResponseStatus
                    {
                        Status = false,
                        Message = "Invalid credentials"
                    });
            }

            var otp = new Random().Next(100000, 999999).ToString();
            _cache.Set(email, otp, TimeSpan.FromMinutes(2));

            await _emailService.SendOtpAsync(email, otp);

            return Ok(new ResponseStatus
            {
                Status = true,
                Message = "Otp sent successfully to your registered email address."
            });
        }

        [HttpPost("set-new-password")]
        public async Task<IActionResult> SetNewPassword(SetNewPassword model)
        {
            if (model == null)
            {
                return BadRequest("Invalid request");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest("No user found");
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.OldPassword,
                model.NewPassword
            );

            if (result.Succeeded)
            {
                return Ok("Password changed successfully");
            }

            return BadRequest(result.Errors.Select(e => e.Description));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(
                    new ResponseStatus
                    {
                        Status = false,
                        Message = "User Not Found."
                    });
            }

            var cachedOtp = _cache.Get<string>(model.Email);

            if (cachedOtp != model.Otp)
            {
                return BadRequest("Invalid OTP");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password reset successfully");
        }

        private string NormalizeEmail(string email)
        {
            var parts = email.Split('@');
            var local = parts[0].Replace(".", "");
            return local + "@" + parts[1];
        }


        private string GenerateJwtToken(ApplicationUser user)
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
