using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.Snappet.API.RequestModel;
using SWD392.Snappet.API.ResponseModel;
using SWD392.Snappet.Repository.BusinessModels;
using SWD392.Snappet.Service.Services;

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
        /// Creates a new user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserResponseModel>> CreateUser([FromBody] UserRequestModel userRequest)
        {
            var user = new User
            {
                Username = userRequest.Username,
                Email = userRequest.Email,
                Password = userRequest.Password, // Ensure you hash this password
                AccountType = userRequest.AccountType
            };

            var createdUserId = await _userService.CreateUserAsync(user);
            var createdUser = await _userService.GetUserByIdAsync(createdUserId);

            var response = new UserResponseModel
            {
                UserId = createdUser.UserId,
                Username = createdUser.Username,
                Email = createdUser.Email,
                AccountType = createdUser.AccountType,
                CreatedAt = createdUser.CreatedAt,
                UpdatedAt = createdUser.UpdatedAt,
                ExpiredDays = createdUser.ExpiredDays,
                Pets = createdUser.Pets.Select(p => new PetResponseModel
                {
                    PetId = p.PetId,
                    PetName = p.PetName,
                    ProfilePhotoUrl = p.ProfilePhotoUrl,
                    PetCategoryName = p.Category.CategoryName,
                    OwnerName = createdUser.Username
                }).ToList()
            };

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, response);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestModel userRequest)
        {
            var user = new User
            {
                UserId = id,
                Username = userRequest.Username,
                Email = userRequest.Email,
                Password = userRequest.Password, // Ensure to hash this
                AccountType = userRequest.AccountType
            };

            await _userService.UpdateUserAsync(user);
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
