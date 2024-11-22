using AdminApp.Models;
using Microsoft.AspNetCore.Identity;

namespace AdminApp.Services;

public class LogoutService(SignInManager<User> signInManager, ILogger<LogoutService> logger)
{
    public async Task Logout()
    {
        await signInManager.SignOutAsync();

        logger.LogInformation("User logged out.");
    }
}
