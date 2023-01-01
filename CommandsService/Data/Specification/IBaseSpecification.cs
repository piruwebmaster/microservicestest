using System.Linq.Expressions;

namespace CommandsService.Data.Specifications
{
    public interface IBaseSpecification<T>
    {
        Expression<Func<T, bool>>? GetFilter();

        Func<IQueryable<T>, IOrderedQueryable<T>>? GetOrderBy();
        string GetIncludeProperties();
    }
}