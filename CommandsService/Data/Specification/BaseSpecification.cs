using System.Linq.Expressions;

namespace CommandsService.Data.Specifications;
public abstract class BaseSpecification<T> : IBaseSpecification<T>
{
    protected Expression<Func<T, bool>>? _filter;

    protected Func<IQueryable<T>, IOrderedQueryable<T>>? _orderBy;
    protected string _includeProperties = string.Empty;

    public Expression<Func<T, bool>>? GetFilter()
    => _filter;

    public string GetIncludeProperties()
    => _includeProperties;

    public Func<IQueryable<T>, IOrderedQueryable<T>>? GetOrderBy()
    => _orderBy;


    protected void setIncldeProperties(string v)
    {
        _includeProperties = v;
    }

    protected void SetOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>>? p)
    {
        _orderBy = p;
    }

    protected void SetFilter(Expression<Func<T, bool>>? p)
    {
        _filter = p;
    }
}

