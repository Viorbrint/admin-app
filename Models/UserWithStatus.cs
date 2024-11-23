using AdminApp.Models;

public class UserWithStatus
{
    public User User { get; set; } = null!;
    public bool IsLockedOut { get; set; }
}
