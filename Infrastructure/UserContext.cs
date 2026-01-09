using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Support> Supports { get; set; }
    public DbSet<Game> Games { get; set; }
    
    public DbSet<Session> Sessions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Configuration pour User ---
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Pseudo)
                .IsRequired()
                .HasMaxLength(100);
            
            // Contrainte d'unicité sur le Pseudo
            builder.HasIndex(x => x.Pseudo).IsUnique();

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(150);
            
            // Contrainte d'unicité sur l'Email
            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(60); // Correspond au BINARY(60)

            builder.Property(x => x.Role)
                .IsRequired()
                .HasMaxLength(5); // Correspond au VARCHAR(5)
        });

        // --- Configuration pour Support ---
        modelBuilder.Entity<Support>(builder =>
        {
            builder.ToTable("Supports");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nom)
                .IsRequired()
                .HasMaxLength(100);

            // Contrainte d'unicité sur le Nom du support
            builder.HasIndex(x => x.Nom).IsUnique();
        });

        // --- Configuration pour Game ---
        modelBuilder.Entity<Game>(builder =>
        {
            builder.ToTable("Games");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nom)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.TempsEstimer)
                .IsRequired();

            // Relation avec Support (Clé étrangère)
            builder.HasOne(g => g.Support)
                .WithMany(s => s.Games)
                .HasForeignKey(g => g.SupportId)
                .OnDelete(DeleteBehavior.Cascade); // Suppression en cascade si un support est supprimé

            // Contrainte unique composite : Nom + SupportId
            // Empêche d'avoir le même jeu deux fois sur le même support
            builder.HasIndex(g => new { g.Nom, g.SupportId }).IsUnique();
        });
        
        modelBuilder.Entity<Session>(builder =>
        {
            builder.ToTable("Sessions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Temps).IsRequired();
            builder.Property(x => x.DateRecord)
                .HasColumnName("Date_record") 
                .IsRequired();

            // Relations
            builder.HasOne(s => s.User)
                .WithMany() 
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Game)
                .WithMany()
                .HasForeignKey(s => s.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}