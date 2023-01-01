using Microsoft.EntityFrameworkCore;
using CommandsService.Models;

namespace CommandsService.Data.Creating;


public static class PlatformConfiguring
{

    public static ModelBuilder AddPlatformConfigure(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Platform>().HasKey(e => e.Id);
        modelBuilder.Entity<Platform>().Property(e => e.Name).IsRequired();
        modelBuilder.Entity<Platform>().Property(e => e.ExternalId).IsRequired();
        modelBuilder.Entity<Platform>().HasMany(e => e.Commands)
        .WithOne(p => p.Platform!)
        .HasForeignKey(p => p.PlatformId);

        return modelBuilder;
    }
}