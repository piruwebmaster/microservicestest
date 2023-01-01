using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Modles;
using PlatformService.SyncDataService.Http;

namespace PlatformService.Controolers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;
    private const string GetPlatformsBydIdName = "GetPlatformsBydIdName";

    public PlatformsController(
        IPlatformRepository repository
        , IMapper mapper
        , ICommandDataClient commandDataClient
        , IMessageBusClient messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetPlatformsAsync()
    {
        var platforms = await _repository.GetAllPlatformsAsync();
        var mapped = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
        return Ok(mapped);
    }

    [HttpGet("{id}", Name = GetPlatformsBydIdName)]
    public async Task<ActionResult<PlatformReadDto>> GetPlatformsByIdAsync(int id)
    {

        var platforms = await _repository.GetPlatformByIdAsync(id);
        if (platforms != default)
        {
            var mapped = _mapper.Map<PlatformReadDto>(platforms);
            return Ok(mapped);
        }
        else
            return NotFound();

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<PlatformReadDto>> DeletePlatformsByIdAsync(int id)
    {
        await _repository.DeletePlatformByIdAsync(id);
        if (await _repository.SaveChangesAsync())
            return Ok();
        else
            return NotFound();
    }


    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatformAsync(PlatformCreateDto platformCreateDto)
    {
        var newEntity = _mapper.Map<Platform>(platformCreateDto);
        await _repository.CreatePlatformAsync(newEntity);
        await _repository.SaveChangesAsync();
        var newEntityResponse = _mapper.Map<PlatformReadDto>(newEntity);

        try
        {
            await _commandDataClient.SendPlatformToCommand(newEntityResponse);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        try
        {
            var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(newEntityResponse) with { Event = "Platform_Published" };
            _messageBusClient.PublishNewPlatform(platformPublishedDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Could not send async message {0}", ex.Message);
        }


        return CreatedAtRoute(GetPlatformsBydIdName, new { Id = newEntityResponse.Id }, newEntityResponse);

    }
}