using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.Snappet.API.RequestModel;
using SWD392.Snappet.API.ResponseModel;
using SWD392.Snappet.Repository.BusinessModels;
using SWD392.Snappet.Service.Services;
using System.Security.Claims;

namespace SWD392.Snappet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets a list of all users.
        /// </summary>
        /// 
        //[HttpGet]
        //public IActionResult GetSecureData()
        //{
        //    return Ok("This is protected data.");
        //}
        [HttpGet]
        public async Task<ActionResult<List<UserResponseModel>>> GetAllUsers([FromQuery] string username = null)
        {
            var users = await _userService.GetAllUsersAsync(username);
            var response = users.Select(u => new UserResponseModel
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                AccountType = u.AccountType,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                ExpiredDays = u.ExpiredDays,
                Pets = u.Pets.Select(p => new PetResponseModel
                {
                    PetId = p.PetId,
                    PetName = p.PetName,
                    ProfilePhotoUrl = p.ProfilePhotoUrl,
                    PetCategoryName = p.Category.CategoryName, // Assuming Category is a navigation property
                    OwnerName = u.Username
                }).ToList() // Populate pet details
            }).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseModel>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var response = new UserResponseModel
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                AccountType = user.AccountType,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                ExpiredDays = user.ExpiredDays,
                Pets = user.Pets.Select(p => new PetResponseModel
                {
                    PetId = p.PetId,
                    PetName = p.PetName,
                    ProfilePhotoUrl = p.ProfilePhotoUrl,
                    PetCategoryName = p.Category.CategoryName,
                    OwnerName = user.Username
                }).ToList()
            };

            return Ok(response);
        }

        

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestModel userRequest)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(); // Return 404 if user doesn't exist
            }

            existingUser.Username = !string.IsNullOrEmpty(userRequest.Username) ? userRequest.Username : existingUser.Username;
            existingUser.Email = !string.IsNullOrEmpty(userRequest.Email) ? userRequest.Email : existingUser.Email;
            existingUser.AccountType = !string.IsNullOrEmpty(userRequest.AccountType) ? userRequest.AccountType : existingUser.AccountType;
            existingUser.UpdatedAt = DateTime.Now;

            await _userService.UpdateUserAsync(existingUser);
            return NoContent();
        }
       


        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
        
    }
}
