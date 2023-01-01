using System.Linq.Expressions;
using CommandsService.Models;

namespace CommandsService.Data.Specifications;
public class GetCommandsWithPlataformsOrderByPlataformName : IBaseSpecification<Command>
{
    private Expression<Func<Command, bool>>? _filter;

    private Func<IQueryable<Command>, IOrderedQueryable<Command>>? _orderBy;
    private string _includeProperties;

    public GetCommandsWithPlataformsOrderByPlataformName(int platformId)
    {
        _filter = (value) => value.PlatformId == platformId;
        _orderBy = (value) => value.OrderBy(command => command.Platform.Name);
        _includeProperties = "Platform";
    }

    public Expression<Func<Command, bool>>? GetFilter()
    => _filter;

    public string GetIncludeProperties()
    => _includeProperties;

    public Func<IQueryable<Command>, IOrderedQueryable<Command>>? GetOrderBy()
    => _orderBy;
}