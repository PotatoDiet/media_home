using CinemaCentral.Services;
using Microsoft.EntityFrameworkCore;

namespace CinemaCentral.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Series> Series => Set<Series>();
    public DbSet<Season> Seasons => Set<Season>();
    public DbSet<Episode> Episodes => Set<Episode>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<User> Users => Set<User>();
    public DbSet<WatchtimeStamp> WatchtimeStamps => Set<WatchtimeStamp>();
    public DbSet<Library> Libraries => Set<Library>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var passwordService = new PasswordService();

        var salt = passwordService.CreateSalt();
        var hash = passwordService.CreateHash("pass"u8.ToArray(), salt);
        modelBuilder
            .Entity<User>()
            .HasData(new User()
            {
                Id = Guid.Parse("f807c6f7-3825-43c4-b48e-db0eb5928b58"),
                Name = "user",
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = UserRole.Admin
            });

        modelBuilder
            .Entity<Library>()
            .HasData(new Library()
            {
                Id = Guid.Parse("ebf247de-d633-468d-86ca-f53080e622be"),
                Name = "Movies",
                Root = "/Users/justin/Workspace/PotatoDiet/Released/CinemaCentral/CinemaCentral/Libraries/Movies"
            });
        modelBuilder
            .Entity<Library>()
            .HasData(new Library()
            {
                Id = Guid.Parse("d32f71f5-9aaf-4d04-9bb7-8b0c045e6604"),
                Name = "TV",
                Root = "/Users/justin/Workspace/PotatoDiet/Released/CinemaCentral/CinemaCentral/Libraries/Series"
            });
    }
}