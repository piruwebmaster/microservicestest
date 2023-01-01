using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Modles;

namespace PlatformService.Profiles;


public class PlatFormsProfile : Profile
{
    public PlatFormsProfile()
    {
        // soruce -> target

        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformReadDto, PlatformPublishedDto>();
        CreateMap<Platform, GrpcPlatformModel>()
            .ForMember(dest => dest.PlatformId,
                (opt => opt.MapFrom(src => src.Id))
            );
    }
}
