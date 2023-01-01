using Microsoft.EntityFrameworkCore;
using CommandsService.Models;
using CommandsService.Data;
using System.Linq.Expressions;
using CommandsService.Data.Specifications;

namespace PlatformsService.Data;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        this._dbSet = context.Set<T>();
    }
    public async Task CreateAsync(T entity)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));
        await _context.AddAsync(entity);
    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }


    public async Task<T?> GetElementAsync(BaseSingleSpecification<T> specification) => (await GetAsync(specification)).FirstOrDefault();

    public async Task<IEnumerable<T>> GetAsync(
            IBaseSpecification<T> specification)
    {
        IQueryable<T> query = _dbSet ?? throw new NullReferenceException(nameof(_dbSet));
        var filter = specification.GetFilter();
        var includeProperties = specification.GetIncludeProperties();
        var orderBy = specification.GetOrderBy();
        

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null && query is not null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            if (query is not null)
            {
                List<T>? result = await query.ToListAsync();
                return result;
            }
            return new List<T>();
        }
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task DeleteByIdAsync(int id)
    {
        T? entityToDelete = await _dbSet.FindAsync(id);
        if (entityToDelete != default)
            Delete(entityToDelete);
    }

    public virtual void Delete(T entityToDelete)
    {
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }
        _dbSet.Remove(entityToDelete);
    }

    public virtual void Update(T entityToUpdate)
    {
        _dbSet.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}