using ProductManagement.Repository;
using ProductManagement.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Service.Services
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
         public async Task<List<User>> GetAllUsersAsyns(string user){         
        // Filter users by username
            var userList = await _unitOfWork.Users.GetAllAsync();
            if (string.IsNullOrEmpty(user))
                return userList;              
            // Return filtered list of users by username
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
                throw new Exception("User not found");
            return await _unitOfWork.Users.RemoveAsync(new User { UserId = id });
        }

    }
   
    
}