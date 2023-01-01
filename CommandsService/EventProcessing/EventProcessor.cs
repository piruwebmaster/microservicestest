using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Data.Specifications;
using CommandsService.Dtos;

namespace CommandsService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(
        IServiceScopeFactory serviceScopeFactory,
        IMapper mapper
        )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;
    }
    public async Task ProccesEvent(string message)
    {
        var eventType = GetEventType(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                await AddPlatform(message); break;
            default: break;
        };



    }
    private async Task AddPlatform(string message)
    {
        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(message);
        if (platformPublishedDto is not null)
        {
            var platform = _mapper.Map<Models.Platform>(platformPublishedDto);
            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
                if (unitOfWork is not null)
                {
                    var platformModelStored = await unitOfWork.PlatformRepository.GetElementAsync(new GetPlatformByExternalId(platform.ExternalId));
                    if (platformModelStored is null)
                    {
                        await unitOfWork.PlatformRepository.CreateAsync(platform);
                        await unitOfWork.SaveAsync();
                    }
                    else
                    {
                        Console.WriteLine("Platform already exist");
                    }
                }
            }
        }


    }

    private EventType GetEventType(string notificationMessage)
    {

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
        if (eventType is not null)
            switch (eventType.Event)
            {
                case "Platform_Published": return EventType.PlatformPublished;
                default: return EventType.Undetermined;
            }
        return EventType.Undetermined;
    }
}
enum EventType
{
    PlatformPublished,
    Undetermined
}