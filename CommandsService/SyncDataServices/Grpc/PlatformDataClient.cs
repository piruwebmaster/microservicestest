using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc;

public class PlatformDataClient : IPlatformDataClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public PlatformDataClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<IEnumerable<Platform>> ReturnAllPlatformsAsync()
    {
        Console.WriteLine("--> calling grpc to get platforms ");
        var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();
        var response = new List<Platform>();
        try
        {
            var rawresponse = await client.GetAllPlatformsAsync(request);
            response = _mapper.Map<IEnumerable<Platform>>(rawresponse.Platform).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("--> could not call Grpc server {0}", ex.Message);

        }

        return response;
    }
}