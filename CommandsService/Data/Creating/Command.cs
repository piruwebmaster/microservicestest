using Microsoft.EntityFrameworkCore;
using CommandsService.Models;

namespace CommandsService.Data.Creating;


public static class CommandConfiguring
{

    public static ModelBuilder AddCommandConfigure(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Command>().HasKey(e => e.Id);
        modelBuilder.Entity<Command>().Property(e => e.HowTo).IsRequired();
        modelBuilder.Entity<Command>().Property(e => e.CommandLine).IsRequired();
        modelBuilder.Entity<Command>().Property(e => e.PlatformId).IsRequired();

        modelBuilder.Entity<Command>().HasOne(p => p.Platform)
        .WithMany(p => p.Commands!)
        .HasForeignKey(p => p.PlatformId);

        return modelBuilder;
    }
}