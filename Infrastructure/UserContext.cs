using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);
            builder.Property(x => x.Pseudo);
            builder.Property(x => x.Email);
            builder.Property(x => x.Password);
            builder.Property(x => x.Role);
        });
    }
}