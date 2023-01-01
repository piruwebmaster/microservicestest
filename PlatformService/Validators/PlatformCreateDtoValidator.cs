using FluentValidation;
using PlatformService.Dtos;
namespace PlatformService.Validators;

public class PlatformCreateDtoValidator : AbstractValidator<PlatformCreateDto>
{

    public PlatformCreateDtoValidator()
    {
        RuleFor(e => e.Name).NotEmpty().NotNull();
        RuleFor(e => e.Cost).NotEmpty().NotNull();
        RuleFor(e => e.Publisher).NotEmpty().NotNull();
    }
}