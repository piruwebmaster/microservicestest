using Microsoft.EntityFrameworkCore;
using PlatformService.Modles;

namespace PlatformService.Data.Creating;


public static class PlatformConfiguring
{

    public static ModelBuilder AddPlatformConfigure(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Platform>().HasKey(e => e.Id);
        modelBuilder.Entity<Platform>().Property(e => e.Name).IsRequired();
        modelBuilder.Entity<Platform>().Property(e => e.Publisher).IsRequired();
        modelBuilder.Entity<Platform>().Property(e => e.Cost).IsRequired();

        return modelBuilder;
    }
}