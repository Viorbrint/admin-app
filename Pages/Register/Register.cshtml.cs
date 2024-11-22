using AdminApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminApp.Pages.Register;

public class RegisterModel : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    private readonly RegisterService _registerService;

    public RegisterModel(RegisterService registerService)
    {
        _registerService = registerService;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ErrorMessage = null;

        if (Input.Password != Input.ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return Page();
        }

        var result = await _registerService.Register(Input.FullName, Input.Email, Input.Password);

        if (result.Succeeded)
        {
            return LocalRedirect(Url.Content("~/login"));
        }
        else
        {
            ErrorMessage =
                result.Errors.FirstOrDefault()?.Description
                ?? "Registration failed. Please try again.";
        }

        return Page();
    }
}
