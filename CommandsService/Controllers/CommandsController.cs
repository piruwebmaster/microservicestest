using AutoMapper;
using CommandsService.Data;
using CommandsService.Data.Specifications;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[ApiController]
[Route("api/c/platforms/{platformId}/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly UnitOfWork _unitofwork;
    private readonly IMapper _mapper;

    public CommandsController(UnitOfWork unitofwork, IMapper mapper)
    {
        _unitofwork = unitofwork ?? throw new ArgumentNullException(nameof(unitofwork));
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetCommandsForPlatformAsync(int platformId)
    {
        if (!await PlatformExist(platformId))
            return NotFound();
        var commands = await _unitofwork.CommandRepository.GetAsync(new GetCommandsWithPlataformsOrderByPlataformName(platformId));
        if (commands != default)
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        else return NotFound();
    }


    [HttpGet("{commandId}", Name = "GetCommandForPlataform")]
    public async Task<ActionResult<CommandReadDto>> GetCommandForPlataformAsync(int platformId, int commandId)
    {
        if (!await PlatformExist(platformId))
            return NotFound();
        var command = await _unitofwork.CommandRepository.GetElementAsync(new GetSingleCommand(platformId, commandId));
        if (command is null) return NotFound();
        return Ok(_mapper.Map<CommandReadDto>(command));
    }

    private async Task<Platform?> GetPlatformAsync(int platformId)
    {
        Console.WriteLine($"--> get commands for platform: {platformId}");
        var platform = await _unitofwork.PlatformRepository.GetByIdAsync(platformId);
        return platform;
    }

    private async Task<bool> PlatformExist(int platformId)
    {
        var platform = await GetPlatformAsync(platformId);
        return platform is null ? false : true;
    }

    [HttpPost]
    public async Task<ActionResult<CommandReadDto>> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
    {
        Console.WriteLine($"--> get commands for platform: {platformId}");
        if (!await PlatformExist(platformId))
            return NotFound();

        var commandModel = _mapper.Map<Command>(commandCreateDto);
        commandModel.PlatformId = platformId;

        await _unitofwork.CommandRepository.CreateAsync(commandModel);
        await _unitofwork.SaveAsync();

        var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

        return CreatedAtRoute("GetCommandForPlataform", new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
    }

}