using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataService.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _endpoint;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _endpoint = _configuration.GetValue<string>("CommandServiceEndoPoint") ?? throw new NullReferenceException("endPoint");
        _httpClient.BaseAddress = new Uri(_endpoint);
    }
    public async Task SendPlatformToCommand(PlatformReadDto platform)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(
                platform),
                 Encoding.UTF8, "application/json");

        var result = await _httpClient.PostAsync("/api/c/Platforms", httpContent);
        if (result.IsSuccessStatusCode)
            Console.WriteLine("sync post success");
        else Console.WriteLine("Sync post error {0}", result.Content);
    }
}