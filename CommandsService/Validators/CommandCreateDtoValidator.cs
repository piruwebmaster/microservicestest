using FluentValidation;
using CommandsService.Dtos;
namespace CommandsService.Validators;

public class CommandCreateDtoValidator : AbstractValidator<CommandCreateDto>
{

    public CommandCreateDtoValidator()
    {
        RuleFor(e => e.HowTo).NotEmpty().NotNull();
        RuleFor(e => e.CommandLine).NotEmpty().NotNull();
    }
}