// Models/User.cs
using System.ComponentModel.DataAnnotations;

namespace RazorWebsite.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Username { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string? PasswordHash { get; set; } // Store hashed passwords
    }
}
