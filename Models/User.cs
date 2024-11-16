namespace AdminApp.Models;

public class User
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public bool IsBlocked { get; set; } = false;

    public ICollection<Login> Logins { get; set; } = [];
}
