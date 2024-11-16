namespace AdminApp.Models;

public class Login
{
    public required int Id { get; set; }
    public required DateTime Time { get; set; }

    public required int UserId { get; set; }
    public required User User { get; set; }
}
