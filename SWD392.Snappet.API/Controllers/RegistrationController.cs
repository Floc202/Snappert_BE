using Microsoft.AspNetCore.Mvc;
using SWD392.Snappet.Service.Services;

namespace SWD392.Snapper.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly RegistrationService _registrationService;

        public RegistrationController(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Register([FromBody] string username, string email, string password, string accountType)
        {
            // Call the registration service directly with simple parameters
            var result = await _registrationService.RegisterUserAsync(username, email, password, accountType);

            if (result == "Email already in use.")
            {
                return BadRequest(result); // Return a bad request if email is already in use
            }

            return Ok(result); // Return success message
        }
    }
}
