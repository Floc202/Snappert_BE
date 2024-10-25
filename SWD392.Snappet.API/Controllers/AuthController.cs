using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SWD392.Snappet.API.Helpers;
using SWD392.Snappet.API.RequestModel;
using SWD392.Snappet.API.ResponseModel;
using SWD392.Snappet.Service.Services;

namespace SWD392.Snappet.API.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _config;

        public AuthController(AuthService authService, IConfiguration config)
        {
            _authService = authService;
            _config = config;
        }

        // Admin Login API
        [HttpPost("admin-login")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginRequestModel adminLoginRequest)
        {
            var admin = await _authService.AuthenticateAdminAsync(adminLoginRequest.Username, adminLoginRequest.Password);

            if (admin == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var token = JwtTokenHelper.GenerateJwtToken(admin, _config["Jwt:Key"]);

            var response = new AdminLoginResponseModel
            {
                Token = token,
                Username = admin.Username,
                Role = admin.Role,
            };

            return Ok(response);
        }
    }
}
