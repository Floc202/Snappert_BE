using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.Snapper.Repository;
using SWD392.Snappet.API.Helpers;
using SWD392.Snappet.API.RequestModel;
using SWD392.Snappet.API.ResponseModel;
using SWD392.Snappet.Repository.BusinessModels;
using SWD392.Snappet.Service.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SWD392.Snapper.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _config;
        private readonly RegistrationService _registrationService;
        private readonly UserService _userService;
        private readonly UnitOfWork _unitOfWork;

        public AuthController(AuthService authService, RegistrationService registrationService, UserService userService, UnitOfWork unitOfWork, IConfiguration config)
        {
            _authService = authService;
            _registrationService = registrationService;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _config = config;
        }

        // Login API
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequest)
        {
            // Kiểm tra xem yêu cầu đăng nhập có hợp lệ không
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { message = "Email and password must be provided." });
            }

            var foundUser = await _authService.checkEmailDuplicated(loginRequest.Email);

            if (foundUser == null)
            {
                return BadRequest(new { message = "Email not found." });
            }

            if (!PasswordHelper.VerifyPassword(loginRequest.Password, foundUser.HashedPassword, foundUser.Salt))
            {
                return Unauthorized();
            }
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }


            // Generate JWT token
            var token = JwtTokenHelper.GenerateJwtToken(loginRequest.Email, _config["Jwt:Key"]);

            // Tạo phản hồi cho đăng nhập
            var response = new LoginResponseModel
            {
                Token = token,
                Username = foundUser.Username,
            };

            return Ok(response);
        }



        // Registration API
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] UserRegistrationRequestModel registrationRequest)
        {
            var foundUser = await _authService.checkEmailDuplicated(registrationRequest.Email);

            if (foundUser != null)
            {
                return BadRequest(new { message = "Email already existed in the system." });
            }

            // Hash the password and generate a salt
            var (hashedPassword, salt) = PasswordHelper.HashPassword(registrationRequest.Password);
            User user = new User
            {
                Username = registrationRequest.Username,
                Email = registrationRequest.Email,
                Password = registrationRequest.Password,
                HashedPassword = hashedPassword,
                Salt = salt,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                AccountType = "Standard",
                ExpiredDays = 0
            };
            await _unitOfWork.Users.CreateAsync(user);
            return Ok(new { Message = "User registered successfully." });
        }

        [HttpGet("signin-google")]
        //[AllowAnonymous]
        public IActionResult SignInGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleCallback") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[AllowAnonymous]
        public async Task<IActionResult> GoogleCallback()
        {
            // Trigger Google authentication if the user isn't already authenticated
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
            {
                return Unauthorized();
            }

            var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var foundUser = await _authService.checkEmailDuplicated(email);

            if (foundUser == null)
            {
                User user = new User
                {
                    Username = name,
                    Email = email,
                    Password = "",
                    HashedPassword = "",
                    Salt = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    AccountType = "Standard",
                    ExpiredDays = 0
                };
                await _unitOfWork.Users.CreateAsync(user);
                var token = JwtTokenHelper.GenerateJwtToken(email, _config["Jwt:Key"]);

                return Ok(new { Token = token });
            }

            // Generate JWT token for the user
            var jwtToken = JwtTokenHelper.GenerateJwtToken(email, _config["Jwt:Key"]);

            return Ok(new { Token = jwtToken });
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel model)
        {
           

            // Check if the old and new passwords are provided
            if (string.IsNullOrWhiteSpace(model.OldPassword) || string.IsNullOrWhiteSpace(model.NewPassword))
            {
                return BadRequest(new { message = "Passwords cannot be empty" });
            }

            try
            {
                if (model == null || model.UserId <= 0 || string.IsNullOrEmpty(model.NewPassword))
                {
                    return BadRequest(new { message = "User ID and new password must be provided." });
                }

                // Get the user by ID
                var user = await _userService.GetUserByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                // Verify the old password using the stored salt and hashed password
                if (!PasswordHelper.VerifyPassword(model.OldPassword, user.HashedPassword, user.Salt))
                {
                    return BadRequest(new { message = "Old password is incorrect" });
                }

                // Hash the new password and update the salt
                var (newHashedPassword, newSalt) = PasswordHelper.HashPassword(model.NewPassword);
                user.HashedPassword = newHashedPassword;
                user.Salt = newSalt;

                // Update the user record in the database
                await _userService.UpdateUserAsync(user);

                return Ok(new { message = "Password updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception details (you can log it using a logging framework)
                return StatusCode(500, new { message = "An error occurred while updating the password", details = ex.Message });
            }
        }



    }
}
