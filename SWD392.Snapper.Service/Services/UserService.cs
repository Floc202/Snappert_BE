using SWD392.Snapper.Repository;
using SWD392.Snapper.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SWD392.Snapper.Service.Services
{

    public class UserService
    {
        public UserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private readonly UnitOfWork _unitOfWork;

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }
        public async Task<List<User>> GetAllUsersAsync(string user)
        {
            var userList = await _unitOfWork.Users.GetAllAsync();  // Ensure this returns IQueryable<User>
            if (string.IsNullOrEmpty(user))
                return userList.ToList();  // Convert IQueryable to List

            return userList.Where(u => u.Username.Contains(user)).ToList();
        }
        


        public async Task<int> CreateUserAsync(User user)
        {
           return await _unitOfWork.Users.CreateAsync(user); 
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            return await _unitOfWork.Users.UpdateAsync(user);
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            // Check if user exists
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                throw new Exception("User not found.");

            // Attach user if not tracked, then remove
            _unitOfWork.Users.Attach(user);
            return await _unitOfWork.Users.RemoveAsync(user);
        }

    }


}
