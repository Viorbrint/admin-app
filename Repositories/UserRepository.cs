using AdminApp.Data;
using AdminApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminApp.Repositories;

public class UserRepository(ApplicationDbContext context)
{
    public async Task<List<User>> GetAll()
    {
        return await context
            .Users.Include(u => u.Logins.OrderByDescending(l => l.Time))
            .ToListAsync();
    }

    public async Task<User?> Get(string id)
    {
        return await context.Users.Include(u => u.Logins).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task Create(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task Update(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task Delete(string id)
    {
        var user = await Get(id);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> Exists(string email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task Block(User user)
    {
        user.IsBlocked = true;
        await context.SaveChangesAsync();
    }

    public async Task Unblock(User user)
    {
        user.IsBlocked = false;
        await context.SaveChangesAsync();
    }
    }
}
