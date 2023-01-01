using Microsoft.EntityFrameworkCore;
using CommandsService.Data.Creating;
using CommandsService.Models;

namespace CommandsService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Command> Commands { get; set; } = default!;
    public DbSet<Platform> Platforms { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddCommandConfigure();
        modelBuilder.AddPlatformConfigure();
    }
}