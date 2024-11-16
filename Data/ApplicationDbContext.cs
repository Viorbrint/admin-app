using AdminApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<Login> Logins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Login>()
            .HasOne(l => l.User)
            .WithMany(u => u.Logins)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
