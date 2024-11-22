using System.ComponentModel.DataAnnotations;

namespace AdminApp.Pages.Login;

public class InputModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; } = null!;
}
