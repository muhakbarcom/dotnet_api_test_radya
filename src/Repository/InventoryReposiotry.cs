using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class InventoryReposiotry : IInventoryRepository
    {
        private readonly ApplicationDBContext _context;

        public InventoryReposiotry(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> AddStockAsync(int bookId, int quantity)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            book.Quantity += quantity;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<Book> ReduceStockAsync(int bookId, int quantity)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            if (book.Quantity < quantity)
            {
                throw new System.Exception("Not enough stock available");
            }

            book.Quantity -= quantity;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}