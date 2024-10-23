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


            //if (foundUser == null || !BCrypt.Net.BCrypt.Verify(password, foundUser.Password))
            //{
            //    return null; // Trả về null nếu không tìm thấy người dùng hoặc mật khẩu không hợp lệ
            //}
            if (foundUser == null || password != foundUser.Password)
            {
                return null; // Invalid login
            }

            return foundUser; // Trả về người dùng nếu xác thực thành công
        }
        public async Task<AdminUser> AuthenticateAdminAsync(string username, string password)
        {
            var adminUsers = await _unitOfWork.AdminUsers.GetAllAsync();
            var admin = adminUsers.FirstOrDefault(a => a.Username == username);

            //if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.Password))
            //{
            //    return null; // Invalid login
            //}
            if (admin == null || password != admin.Password)
            {
                return null; // Invalid login
            }

            return admin;
        }


    }
}
