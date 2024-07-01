
using Models;

namespace Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string userId);
        Task<User> GetByEmailAsync(string email);
        Task<User> UpdateAsync(User user);
        Task<User> DeleteAsync(User user);
        Task<User> UpdateProfileAsync(User user);
        Task<bool> isRoleExistsAsync(string role);
        Task<User> GetByUserNameAsync(string userName);
    }
}