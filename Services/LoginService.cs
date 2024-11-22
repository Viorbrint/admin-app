using AdminApp.Models;
using Microsoft.AspNetCore.Identity;

namespace AdminApp.Services;

public class LoginService(
    UserService userService,
    SignInManager<User> signInManager,
    ILogger<LoginService> logger
)
{
    public async Task<SignInResult> Login(string email, string password)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(email);

        if (user == null)
        {
            return SignInResult.Failed;
        }

        var result = await signInManager.PasswordSignInAsync(
            user,
            password,
            isPersistent: true,
            lockoutOnFailure: false
        );

        if (result.Succeeded)
        {
            await userService.AddLogin(user.Id);
            logger.LogInformation("User logged in.");
        }

        return result;
    }
}
