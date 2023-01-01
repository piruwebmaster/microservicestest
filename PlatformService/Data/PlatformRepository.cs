using Microsoft.EntityFrameworkCore;
using PlatformService.Modles;

namespace PlatformService.Data;

public class PlatformRepository : IPlatformRepository
{
    private readonly AppDbContext _context;

    public PlatformRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task CreatePlatformAsync(Platform platform)
    {
        platform = platform ?? throw new ArgumentNullException(nameof(platform));
        await _context.AddAsync(platform);
    }

    public async Task DeletePlatformByIdAsync(int id)
    {
        var entity = await _context.Platforms.FirstOrDefaultAsync(e => e.Id == id);
        if (entity != default)
            _context.Entry(entity).State = EntityState.Deleted;
    }

    public async Task<IEnumerable<Platform>> GetAllPlatformsAsync()
    {
        return await _context.Platforms.ToListAsync();
    }


    public async Task<Platform?> GetPlatformByIdAsync(int id)
    {
        var platform = await _context.Platforms.FirstOrDefaultAsync(e => e.Id == id);
        return platform;

    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}