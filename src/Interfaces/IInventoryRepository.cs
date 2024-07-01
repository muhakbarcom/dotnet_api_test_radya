

using Models;

namespace Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> AddStockAsync(int bookId, int quantity);
        Task<Book> ReduceStockAsync(int bookId, int quantity);
        Task<bool> DeleteBookAsync(int bookId);
    }
}