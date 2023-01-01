using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataService.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepository _platformRespository;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepository platformRespository, IMapper mapper)
    {
        _platformRespository = platformRespository;
        _mapper = mapper;
    }

    public async override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var response = new PlatformResponse();
        var platforms = await _platformRespository.GetAllPlatformsAsync();

        foreach (var plat in platforms)
        {
            var mapped =_mapper.Map<GrpcPlatformModel>(plat);
            response.Platform.Add(mapped);
        }
        return response;
    }
}