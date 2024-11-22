using AdminApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminApp.Pages.Logout;

public class LogoutModel(LogoutService logoutService) : PageModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        await logoutService.Logout();
        return LocalRedirect(Url.Content("~/login"));
    }
}
