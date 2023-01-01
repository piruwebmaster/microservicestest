using Microsoft.Extensions.Logging;
using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data;

public static class PrepDb
{
    public static async Task UsePrepPopulation(this IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateAsyncScope())
        {

            var context = serviceScope.ServiceProvider.GetService<AppDbContext>() ?? throw new NullReferenceException(nameof(AppDbContext));
            var client = serviceScope.ServiceProvider.GetService<IPlatformDataClient>() ?? throw new NullReferenceException(nameof(AppDbContext));
            var platforms = await client.ReturnAllPlatformsAsync();
            await SeedData(context, platforms);
        };
    }

    private async static Task SeedData(AppDbContext context, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("seeding data");

        foreach (var plat in platforms)
        {
            if (!context.Platforms.Any(platform => platform.Id == plat.ExternalId))
            {
                context.Platforms.Add(plat);
            }

        }
        await context.SaveChangesAsync();
    }
}