

using Models;

namespace Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> PlaceOrderAsync(string userId);
        Task<IEnumerable<Order>> GetOrdersAsync(string userId);
    }
}