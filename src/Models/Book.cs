
namespace Models
{
    public class Book
    {
        public int Id { get; set; }  // Add a primary key
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<ShoppingCart> ShoppingCarts { get; set; }
    }
}