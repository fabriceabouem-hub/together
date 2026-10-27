using Microsoft.EntityFrameworkCore;
using together.Api.Model;  
namespace together.Api.Data;

public class AppDbContext : DbContext
{
public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Sport> Sports => Set<Sport>();
    public DbSet<SportGroup> Groups => Set<SportGroup>();
    public DbSet<Membership> Memberships => Set<Membership>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Clé composée : un utilisateur ne peut rejoindre un groupe qu'une fois
        modelBuilder.Entity<Membership>()
            .HasKey(m => new { m.UserId, m.GroupId });

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<SportGroup>()
            .HasOne(g => g.Creator)
            .WithMany()
            .HasForeignKey(g => g.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Sports de départ
        modelBuilder.Entity<Sport>().HasData(
            new Sport { Id = 1, Name = "Football" },
            new Sport { Id = 2, Name = "Basketball" },
            new Sport { Id = 3, Name = "Course à pied" },
            new Sport { Id = 4, Name = "Tennis" },
            new Sport { Id = 5, Name = "Cyclisme" },
            new Sport { Id = 6, Name = "Volleyball" });
    }
}
