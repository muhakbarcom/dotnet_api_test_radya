

using Models;

namespace Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> AddToCartAsync(string userId, int bookId, int quantity);
        Task<ShoppingCart> UpdateCartAsync(int id, int quantity);
        Task<bool> RemoveFromCartAsync(int id);
        Task<IEnumerable<ShoppingCart>> GetCartItemsAsync(string userId);
    }
}