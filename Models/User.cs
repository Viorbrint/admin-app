using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AdminApp.Models
{
    public class User : IdentityUser
    {
        public bool IsBlocked { get; set; } = false;

        [Required]
        public override string? UserName { get; set; }

        [Required]
        public override string? Email { get; set; }

        [Required]
        public override string? PasswordHash { get; set; }

        public ICollection<Login> Logins { get; set; } = [];
    }
}
