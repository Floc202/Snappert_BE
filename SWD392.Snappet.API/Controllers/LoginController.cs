using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SWD392.Snapper.Repository;
using SWD392.Snappet.API.Helpers;
using SWD392.Snappet.API.RequestModel;
using SWD392.Snappet.API.ResponseModel;
using SWD392.Snappet.Repository.BusinessModels;
using SWD392.Snappet.Service.Services;
using System.Security.Claims;

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
        //        [HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequest)
        //{
        //            var users= await _unitOfWork.Users.GetAllAsync();

        //var foundUser = users.FirstOrDefault(u => u.Email == loginRequest.Email);
        //            if (foundUser == null)
        //            {
        //                return Unauthorized(new { message = "Email not found." });
        //            }
        //            /// sau khi đưa vào sẽ kiểm tra null user
        //            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, foundUser.Password);
        //            if (!isPasswordValid)
        //            {
        //                return Unauthorized(new { message = "Invalid password." });
        //            }
        //            ////
        //            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
        //    {
        //        return BadRequest(new { message = "Email and password must be provided." });
        //    }

        //    var token = JwtTokenHelper.GenerateJwtToken(foundUser, _config["Jwt:Key"]);

        //    var response = new LoginResponseModel
        //    {
        //        Token = token,
        //        Username = foundUser.Username,
        //    };

        //    return Ok(response);
        //}
        // Login API
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequest)
        {
            // Kiểm tra xem yêu cầu đăng nhập có hợp lệ không
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { message = "Email and password must be provided." });
            }

            // Sử dụng AuthService để xác thực người dùng
            var foundUser = await _authService.AuthenticateUserAsync(loginRequest.Email, loginRequest.Password);

            if (foundUser == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Tạo token JWT cho người dùng đã xác thực
            var token = JwtTokenHelper.GenerateJwtToken(foundUser, _config["Jwt:Key"]);

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
            var result = await _registrationService.RegisterUserAsync(
            registrationRequest.Username, registrationRequest.Email, registrationRequest.Username, registrationRequest.AccountType);

            if (result == "Email already in use.")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel model)
        {
            // Extract the User ID from the JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest(new { message = "Invalid user ID" });
            }

            // Get the user by ID
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Verify the old password

            //if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
            //{
            //    return BadRequest(new { message = "Old password is incorrect" });
            //}
            if (model.OldPassword != user.Password)
            {
                 return BadRequest(new { message = "Old password is incorrect" });
            }

            // Hash the new password
            //user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            user.Password = model.NewPassword;

            // Update the user record in the database
            await _userService.UpdateUserAsync(user);

            return Ok(new { message = "Password updated successfully" });
        }

    }
}
