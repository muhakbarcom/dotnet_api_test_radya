using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Order> Orders { get; set; }
        public List<ShoppingCart> ShoppingCarts { get; set; }
    }
}