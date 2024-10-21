using Microsoft.AspNetCore.Mvc;
using SWD392.Snappet.API.Helpers;
using SWD392.Snappet.API.RequestModel;
using SWD392.Snappet.API.ResponseModel;
using SWD392.Snappet.Service.Services;

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

        public AuthController(AuthService authService, RegistrationService registrationService, UserService userService, IConfiguration config)
        {
            _authService = authService;
            _registrationService = registrationService;
            _userService = userService;
            _config = config;
        }

        // Login API
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequest)
        {
            var user = await _authService.AuthenticateUserAsync(loginRequest.Email, loginRequest.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var token = JwtTokenHelper.GenerateJwtToken(user, _config["Jwt:Key"]);

            var response = new LoginResponseModel
            {
                Token = token
            };

            return Ok(response);
        }

        // Registration API
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] UserRegistrationRequestModel registrationRequest)
        {
            var result = await _registrationService.RegisterUserAsync(
            registrationRequest.Username, registrationRequest.Email, registrationRequest.Username, registrationRequest.AccountType);

            if (result == "Email already in use.")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        //[HttpPut("change-password")]
        //public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel model)
        //{   
        //    if (!int.TryParse(User.Identity.Name, out int userId))
        //    {
        //        return BadRequest(new { message = "Invalid user ID" });
        //    } 
        //    var user = await _userService.GetUserByIdAsync(userId);
        //    // Verify the old password
        //    if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
        //    {
        //        return BadRequest(new { message = "Old password is incorrect" });
        //    }
        //    // Hash the new password
        //    user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

        //    // Update the user record in the database
        //    await _userService.UpdateUserAsync(user);

        //    return Ok(new { message = "Password updated successfully" });
        //}

    }
}
