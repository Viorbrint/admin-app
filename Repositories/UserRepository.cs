using AdminApp.Data;
using AdminApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminApp.Repositories;

public class UserRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<User>> GetAll()
    {
        return await _context.Users.Include(u => u.Logins).ToListAsync();
    }

    public async Task<User?> Get(int id)
    {
        return await _context.Users.Include(u => u.Logins).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task Create(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task Update(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var user = await Get(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> Exists(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task Block(User user)
    {
        user.IsBlocked = true;
        await _context.SaveChangesAsync();
    }

    public async Task Unblock(User user)
    {
        user.IsBlocked = false;
        await _context.SaveChangesAsync();
    }
}
