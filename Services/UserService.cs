using System.Security.Claims;
using AdminApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminApp.Services;

public class UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
{
    public async Task<List<UserWithStatus>> SearchUsersAsync(string searchTerm)
    {
        var normalizedSearchTerm = searchTerm.ToUpper();
        var filteredUsers = await userManager
            .Users.Include(u => u.Logins.OrderByDescending(l => l.Time))
            .OrderByDescending(u => u.Logins.Any() ? u.Logins.Max(l => l.Time) : DateTime.MinValue)
            .Where(user =>
                user.Email != null && user.Email.Contains(normalizedSearchTerm)
                || user.UserName != null && user.UserName.Contains(normalizedSearchTerm)
                || user.NormalizedEmail != null
                    && user.NormalizedEmail.Contains(normalizedSearchTerm)
            )
            .ToListAsync();

        var result = new List<UserWithStatus>();

        foreach (var user in filteredUsers)
        {
            var isLockedOut = await userManager.IsLockedOutAsync(user);
            result.Add(new UserWithStatus { User = user, IsLockedOut = isLockedOut });
        }

        return result;
    }

    public async Task<bool> Delete(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        var result = await userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> Block(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> Unblock(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        user.LockoutEnd = null;
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> AddLogin(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        Login login = new() { UserId = user.Id, User = user };
        user.Logins.Add(login);

        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<User?> GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity?.IsAuthenticated == true)
            return null;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return null;

        return await userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<bool> IsLockedOut(User user)
    {
        return await userManager.IsLockedOutAsync(user);
    }

    public async Task<bool> IsAccessDenied()
    {
        var user = await GetCurrentUser();
        if (user == null || user.LockoutEnd != null)
        {
            return true;
        }

        return false;
    }
}
