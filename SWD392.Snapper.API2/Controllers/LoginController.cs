using Microsoft.AspNetCore.Mvc;
using SWD392.Snapper.Service.ResponseModel;
using SWD392.Snapper.Service.Services;
using SWD392.Snapper.Service.RequestModel;
using SWD392.Snapper.Service.Helpers;


[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IConfiguration _config;

    public AuthController(AuthService authService, IConfiguration config)
    {
        _authService = authService;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequest)
    {
        var user = await _authService.AuthenticateUserAsync(loginRequest.Email, loginRequest.Password);

        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        var token = JwtTokenHelper.GenerateJwtToken(user, _config["Jwt:Key"]);

        var response = new LoginResponseModel
        {
            Token = token,
            Username = user.Username
        };

        return Ok(response);
    }
 

}
