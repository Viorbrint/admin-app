using AdminApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminApp.Pages.Login;

public class LoginModel(LoginService loginService) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = null!;

    public string? LoginErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var result = await loginService.Login(Input.Email, Input.Password);
        if (result.Succeeded)
        {
            return LocalRedirect(Url.Content("~/"));
        }
        else
        {
            LoginErrorMessage = "Invalid email or password";
            return Page();
        }
    }
}
