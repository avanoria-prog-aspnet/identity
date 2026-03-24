using Microsoft.EntityFrameworkCore;
using Presentation.WebApp.Identity;

namespace Presentation.WebApp.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(e =>
        {
            e.ToTable("Users");

            e.HasKey(e => e.Id);

            e.Property(e => e.Email)
                .IsRequired();

            e.Property(e => e.HashedPassword)
                .IsRequired();

            e.Property(e => e.HashPasswordSalt)
                .IsRequired();

            e.HasIndex(e => e.Email)
                .IsUnique();

        });
    }
}


