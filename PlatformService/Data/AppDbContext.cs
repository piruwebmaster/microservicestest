using Microsoft.EntityFrameworkCore;
using PlatformService.Data.Creating;
using PlatformService.Modles;

namespace PlatformService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Platform> Platforms { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddPlatformConfigure();
    }
}