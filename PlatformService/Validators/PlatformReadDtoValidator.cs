using FluentValidation;
using PlatformService.Dtos;
namespace PlatformService.Validators;

public class PlatformReadDtoValidator : AbstractValidator<PlatformReadDto>
{

    public PlatformReadDtoValidator()
    {


        RuleFor(e => e.Id).NotEmpty().WithMessage("Name is required");
        RuleFor(e => e.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(e => e.Cost).NotEmpty().WithMessage("cost is required");
        RuleFor(e => e.Publisher).NotEmpty().WithMessage("Publisher is required");
    }
}