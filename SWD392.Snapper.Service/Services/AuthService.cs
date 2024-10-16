using SWD392.Snapper.Repository.Models;
using SWD392.Snapper.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWD392.Snapper.Service.Services
{
    public class AuthService
    {
        private readonly UnitOfWork _unitOfWork;

        public AuthService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            var user = await _unitOfWork.Users.GetAll()
        .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return null;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            // If the password doesn't match, return null
            if (!isPasswordValid)
            {
                return null;
            }
            return user;
        }

    }
}
