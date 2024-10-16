using SWD392.Snapper.Repository.Models;
using SWD392.Snapper.Repository;
using Shared.RequestModel;
using Snappet_Be.ResponseModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWD392.Snapper.Service.Services
{
    public class RegistrationService
    {
        private readonly UnitOfWork _unitOfWork;

        public RegistrationService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserRegistrationResponseModel> RegisterUserAsync(UserRegistrationRequestModel request)
        {
            var existingUser = await _unitOfWork.Users.GetAll()
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return new UserRegistrationResponseModel
                {
                    Message = "Email already in use."
                };
            }

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                //AccountType = request.AccountType,
                AccountType = "Standard",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.CreateAsync(newUser);

            return new UserRegistrationResponseModel
            {
                UserId = newUser.UserId,
                Username = newUser.Username,
                Email = newUser.Email,
                Message = "Registration successful."
            };
        }
    }
}
