
using System.ComponentModel.DataAnnotations;

namespace Dtos.User
{
    public class UpdateUserDto
    {
        [Required]
        public string? Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
    }
}