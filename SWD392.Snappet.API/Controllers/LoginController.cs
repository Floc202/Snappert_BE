using Microsoft.AspNetCore.Mvc;
using SWD392.Snappet.API.Helpers;
using SWD392.Snappet.API.RequestModel;
using SWD392.Snappet.API.ResponseModel;
using SWD392.Snappet.Service.Services;

[Route("api/[controller]")]
[ApiController]
public class LoginController(AuthService authService, IConfiguration config) : ControllerBase
{
    private readonly AuthService _authService = authService;
    private readonly IConfiguration _config = config;

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


}
