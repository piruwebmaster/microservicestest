using System.Linq.Expressions;
using CommandsService.Data.Specifications;
using CommandsService.Models;

namespace CommandsService.Data;

public interface IRepository<T>
{
    Task<bool> SaveChangesAsync();
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int Id);
    Task CreateAsync(T type);
    Task DeleteByIdAsync(int id);

    void Delete(T entityToDelete);

    void Update(T entityToUpdate);

    Task<IEnumerable<T>> GetAsync(IBaseSpecification<T> especification);
    Task<T?> GetElementAsync(BaseSingleSpecification<T> specification);
}