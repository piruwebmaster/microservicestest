using PlatformService.Modles;

namespace PlatformService.Data;

public interface IPlatformRepository
{
    Task<bool> SaveChangesAsync();
    Task<IEnumerable<Platform>> GetAllPlatformsAsync();
    Task<Platform?> GetPlatformByIdAsync(int Id);
    Task CreatePlatformAsync(Platform platform);
    Task DeletePlatformByIdAsync(int id);
}