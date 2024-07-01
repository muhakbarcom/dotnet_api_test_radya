
namespace Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }  // Add a primary key
        public string UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}