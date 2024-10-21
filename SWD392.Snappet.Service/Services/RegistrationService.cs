using SWD392.Snapper.Repository;
using SWD392.Snappet.Repository.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Snappet.Service.Services
{
    public class RegistrationService
    {
        private readonly UnitOfWork _unitOfWork;

        public RegistrationService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> RegisterUserAsync(string username, string email, string password, string accountType)
        {
            // Check if the user already exists
            var users = await _unitOfWork.Users.GetAllAsync();
            var existingUser = users.FirstOrDefault(u => u.Email == email);

            if (existingUser != null)
            {
                return "Email already in use."; // Return error message
            }

            // Create a new User object
            var newUser = new User
            {
                Username = username,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password), // Hash the password
                AccountType = accountType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save the new user to the database
            await _unitOfWork.Users.CreateAsync(newUser);

            return "Registration successful."; // Return success message
        }
    }

}
