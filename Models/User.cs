using Microsoft.AspNetCore.Identity;

namespace AdminApp.Models
{
    public class User : IdentityUser
    {
        public ICollection<Login> Logins { get; set; } = [];
    }
}
