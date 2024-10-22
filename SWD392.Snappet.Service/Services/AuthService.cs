using Org.BouncyCastle.Crypto.Generators;
using SWD392.Snapper.Repository;
using SWD392.Snappet.Repository.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SWD392.Snappet.Service.Services
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
            var users = await _unitOfWork.Users.GetAllAsync();
            var foundUser = users.FirstOrDefault(u => u.Email == email);
            

            if (foundUser == null || !BCrypt.Net.BCrypt.Verify(password, foundUser.Password))
            {
                return null; // Trả về null nếu không tìm thấy người dùng hoặc mật khẩu không hợp lệ
            }

            return foundUser; // Trả về người dùng nếu xác thực thành công
        }



    }
}
