
using Models;

namespace Interfaces
{
    public interface iTokenService
    {
        Task<string> CreateToken(User user);
    }
}