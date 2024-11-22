using AdminApp.Models;
using Microsoft.AspNetCore.Identity;

namespace AdminApp.Services;

public class RegisterService(UserManager<User> userManager, ILogger<RegisterService> logger)
{
    public async Task<IdentityResult> Register(string fullName, string email, string password)
    {
        logger.LogInformation(
            $"Trying to register user with email {email} and password {password}"
        );
        var user = new User { UserName = fullName, Email = email };

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            logger.LogInformation("User created a new account");
        }

        return result;
    }
}
