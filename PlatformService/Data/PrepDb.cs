using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlatformService.Modles;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void UsePrepPopulation(this IApplicationBuilder applicationBuilder, bool isProduction)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateAsyncScope())
        {

            var context = serviceScope.ServiceProvider.GetService<AppDbContext>() ?? throw new NullReferenceException(nameof(AppDbContext));
            SeedData(context, isProduction);
        };
    }

    private static void SeedData(AppDbContext context, bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("---> attempting to apply migrations .....");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error on migration {ex.Message}");
            }
        }

        if (!context.Platforms.Any())
        {
            Console.WriteLine("seeding data");

            context.Platforms.AddRangeAsync(
                new Platform("Dotnet", "Microsoft", "free"),
                new Platform("SQL Server Express", "Microsoft", "free"),
                new Platform("Kubernetes", "Cloud Native Computing Foundation", "free")
            );

            context.SaveChanges();
        }
        else

            Console.WriteLine("We already have data");
    }
}