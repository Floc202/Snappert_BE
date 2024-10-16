using Microsoft.AspNetCore.Mvc;
using SWD392.Snapper.Service.ResponseModel;
using SWD392.Snapper.Service.Services;

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
        public async Task<ActionResult<UserRegistrationResponseModel>> Register([FromBody] Shared.RequestModel.UserRegistrationRequestModel registrationRequest)
        {
            var response = await _registrationService.RegisterUserAsync(registrationRequest);

            if (response.Message == "Email already in use.")
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
