
using CommandsService.Models;
using PlatformsService.Data;

namespace CommandsService.Data;
public class UnitOfWork : IDisposable
{
    private AppDbContext _context;
    private Repository<Command> commandRepository = default!;
    private Repository<Platform> platformRepository = default!;

    public UnitOfWork(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Repository<Platform> PlatformRepository
    {
        get
        {
            this.platformRepository ??= new Repository<Platform>(_context);

            return platformRepository;
        }
    }

    public Repository<Command> CommandRepository
    {
        get
        {

            this.commandRepository ??= this.commandRepository = new Repository<Command>(_context);
            return commandRepository;
        }
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}