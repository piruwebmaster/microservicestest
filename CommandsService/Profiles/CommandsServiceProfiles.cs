using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatformService;

namespace CommandsService.Profiles;


public class CommandsServiceProfiles : Profile
{
    public CommandsServiceProfiles()
    {
        // source -> target

        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(destination => destination.ExternalId, opt => opt.MapFrom(src => src.Id));
        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(dest => dest.ExternalId, cnf => cnf.MapFrom(src => src.Id));
        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.ExternalId, cnf => cnf.MapFrom(src => src.PlatformId));

    }
}
