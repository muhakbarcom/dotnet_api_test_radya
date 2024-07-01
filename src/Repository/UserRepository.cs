using Data;
using Dtos.User;
using Helpers;
using Interfaces;
using Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Task<User> DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<bool> isRoleExistsAsync(string role)
        {
            return await _userManager.FindByNameAsync(role) != null;
        }

        public Task<User> UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateProfileAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}