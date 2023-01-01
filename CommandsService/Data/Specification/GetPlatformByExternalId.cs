using System.Linq.Expressions;
using CommandsService.Models;

namespace CommandsService.Data.Specifications;
public class GetPlatformByExternalId : BaseSingleSpecification<Platform>
{
    public GetPlatformByExternalId(int externalPlatformId)
    {
        SetFilter((value) => (value.ExternalId == externalPlatformId));
    }
}