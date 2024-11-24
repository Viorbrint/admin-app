using AdminApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<User>(options)
{
    public required DbSet<Login> Logins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Login>(entity =>
        {
            entity
                .HasOne(l => l.User)
                .WithMany(u => u.Logins)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(l => l.UserId).IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.NormalizedEmail).IsUnique();
        });
    }
}
