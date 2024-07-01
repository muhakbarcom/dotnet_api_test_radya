
namespace Models
{
    public class Order
    {
        public int Id { get; set; }  // Add a primary key
        public string UserId { get; set; }
        public User User { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderNumber { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}