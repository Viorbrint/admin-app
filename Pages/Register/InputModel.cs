using System.ComponentModel.DataAnnotations;

namespace AdminApp.Pages.Register;

public class InputModel
{
    [Required]
    public string? FullName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required]
    public string? ConfirmPassword { get; set; }
}
