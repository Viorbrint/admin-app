namespace AdminApp.Models;

public class Login
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime Time { get; set; } = DateTime.Now.ToUniversalTime();

    public required string UserId { get; set; }
    public required User User { get; set; }
}
