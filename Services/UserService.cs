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
                user.Email.Contains(normalizedSearchTerm)
                || user.UserName.Contains(normalizedSearchTerm)
                || user.NormalizedEmail.Contains(normalizedSearchTerm)
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

    public async Task<List<UserWithStatus>> GetAllWithStatus()
    {
        var users = await userManager
            .Users.Include(u => u.Logins.OrderByDescending(l => l.Time))
            .OrderByDescending(u => u.Logins.Any() ? u.Logins.Max(l => l.Time) : DateTime.MinValue)
            .ToListAsync();
        var result = new List<UserWithStatus>();

        foreach (var user in users)
        {
            var isLockedOut = await userManager.IsLockedOutAsync(user);
            result.Add(new UserWithStatus { User = user, IsLockedOut = isLockedOut });
        }

        return result;
    }

    public async Task<List<User>> GetAll()
    {
        return await userManager.Users.Include(u => u.Logins).ToListAsync();
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

        // TODO: Literal
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
        return await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User);
    }

    public async Task<bool> IsLockedOut(User user)
    {
        return await userManager.IsLockedOutAsync(user);
    }
}
