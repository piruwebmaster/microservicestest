using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Controllers;

[ApiController]
[Route("api/c/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private const string GetCommandBydIdName = "GetCommandBydIdName";

    public PlatformsController(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetPlatformAsync()
    {
        Console.WriteLine("--> Getting platforms form commandsService");
        var platformsItems = await _unitOfWork.PlatformRepository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformsItems));
    }

    [HttpPost]
    public IActionResult TestInboundConnection()
    {
        Console.WriteLine("success inbound connection");
        return Ok();
    }

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetCommandsAsync()
    // {
    //     var command = await _repository.GetAllCommandsAsync();
    //     var mapped = _mapper.Map<IEnumerable<CommandReadDto>>(command);
    //     return Ok(mapped);
    // }

    // [HttpGet("{id}", Name = GetCommandBydIdName)]
    // public async Task<ActionResult<CommandReadDto>> GetCommandByIdAsync(int id)
    // {

    //     var commands = await _repository.GetCommandByIdAsync(id);
    //     if (commands != default)
    //     {
    //         var mapped = _mapper.Map<CommandReadDto>(commands);
    //         return Ok(mapped);
    //     }
    //     else
    //         return NotFound();

    // }

    // [HttpDelete("{id}")]
    // public async Task<ActionResult<CommandReadDto>> DeleteCommandsByIdAsync(int id)
    // {
    //     await _repository.DeleteCommandByIdAsync(id);
    //     if (await _repository.SaveChangesAsync())
    //         return Ok();
    //     else
    //         return NotFound();
    // }


    // [HttpPost]
    // public async Task<ActionResult<CommandReadDto>> CreateCommandAsync(CommandCreateDto commandCreateDto)
    // {
    //     var newEntity = _mapper.Map<Command>(commandCreateDto);
    //     await _repository.CreateCommandAsync(newEntity);
    //     await _repository.SaveChangesAsync();
    //     var newEntityResponse = _mapper.Map<CommandReadDto>(newEntity);

    //     return CreatedAtRoute(GetCommandBydIdName, new { Id = newEntityResponse.Id }, newEntityResponse);

    // }
}