using System;
using System.ComponentModel.DataAnnotations;
namespace CoworkingBooking.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
