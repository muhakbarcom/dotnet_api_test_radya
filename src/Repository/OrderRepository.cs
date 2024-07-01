
using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _context;

        public OrderRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Order> PlaceOrderAsync(string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cartItems = await _context.ShoppingCarts.Include(sc => sc.Book).Where(c => c.UserId == userId).ToListAsync();


                if (!cartItems.Any())
                {
                    throw new Exception("Your cart is empty");
                }

                decimal totalPrice = 0;

                foreach (var item in cartItems)
                {
                    totalPrice += item.Book.Price * item.Quantity;
                }

                var order = new Order
                {
                    UserId = userId,
                    Status = "completed",
                    TotalPrice = totalPrice,
                    OrderNumber = $"ORDER-{Guid.NewGuid().ToString().ToUpper()}"
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();


                foreach (var item in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        Price = item.Book.Price
                    };

                    _context.OrderItems.Add(orderItem);

                    var book = await _context.Books.FindAsync(item.BookId);
                    book.Quantity -= item.Quantity;
                    _context.Books.Update(book);
                }

                _context.ShoppingCarts.RemoveRange(cartItems);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(string userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).Include(o => o.OrderItems).ToListAsync();
        }
    }
}