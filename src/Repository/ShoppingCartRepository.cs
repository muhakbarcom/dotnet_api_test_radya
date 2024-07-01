using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDBContext _context;

        public ShoppingCartRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart> AddToCartAsync(string userId, int bookId, int quantity)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            var cartItem = await _context.ShoppingCarts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId);

            if (cartItem != null)
            {
                if (quantity + cartItem.Quantity > book.Quantity)
                {
                    throw new System.Exception("Book stock is not enough");
                }

                cartItem.Quantity += quantity;
                _context.ShoppingCarts.Update(cartItem);
            }
            else
            {
                if (quantity > book.Quantity)
                {
                    throw new System.Exception("Book stock is not enough");
                }

                cartItem = new ShoppingCart
                {
                    UserId = userId,
                    BookId = bookId,
                    Quantity = quantity
                };

                _context.ShoppingCarts.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<ShoppingCart> UpdateCartAsync(int id, int quantity)
        {
            var cartItem = await _context.ShoppingCarts.FindAsync(id);
            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart item not found");
            }

            var book = await _context.Books.FindAsync(cartItem.BookId);
            if (quantity > book.Quantity)
            {
                throw new System.Exception("Book stock is not enough");
            }

            cartItem.Quantity = quantity;
            _context.ShoppingCarts.Update(cartItem);
            await _context.SaveChangesAsync();

            return cartItem;
        }

        public async Task<bool> RemoveFromCartAsync(int id)
        {
            var cartItem = await _context.ShoppingCarts.FindAsync(id);
            if (cartItem == null)
            {
                return false;
            }

            _context.ShoppingCarts.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ShoppingCart>> GetCartItemsAsync(string userId)
        {
            return await _context.ShoppingCarts
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
    }
}