using System.Linq.Expressions;
using CommandsService.Models;

namespace CommandsService.Data.Specifications;
public class GetSingleCommand : BaseSingleSpecification<Command>
{
    public GetSingleCommand(int platformId, int commandId)
    {
        SetFilter((value) => (value.PlatformId == platformId && value.Id == commandId));
        SetOrderBy((value) => value.OrderBy(command => command.Platform.Name));
        setIncldeProperties("Platform");
    }
}