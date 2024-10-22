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

        //public async Task<User> AuthenticateUserAsync(string email, string password)
        //{
        //    var users = await _unitOfWork.Users.GetAllAsync();
        //    var foundUser = users.FirstOrDefault(u => u.Email == email);

        //    // Kiểm tra xem người dùng có tồn tại hay không
        //    if (foundUser == null)
        //    {
        //        Console.WriteLine($"User not found for email: {email}"); // Ghi lại thông tin
        //        return null; // Nếu không tìm thấy người dùng
        //    }

        //    // Kiểm tra mật khẩu
        //    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password.Trim(), foundUser.Password);
        //    if (!isPasswordValid)
        //    {
        //        Console.WriteLine("Invalid password entered for email: " + email); // Ghi lại thông tin
        //        return null;
        //    }

        //    Console.WriteLine("User authenticated successfully for email: " + email); // Ghi lại thông tin
        //    return foundUser;
        //}
        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var foundUser = users.FirstOrDefault(u => u.Email == email);
            Console.WriteLine($"Mật khẩu nhập vào: {password}");
            Console.WriteLine($"Chuỗi băm lưu trữ: {foundUser.Password}");

            if (foundUser == null || !BCrypt.Net.BCrypt.Verify(password, foundUser.Password))
            {
                return null; // Trả về null nếu không tìm thấy người dùng hoặc mật khẩu không hợp lệ
            }

            return foundUser; // Trả về người dùng nếu xác thực thành công
        }



    }
}
