using FluentValidation;

namespace IPL.SOLID.API.Application.Commands.Applications.CreateApplication;

public sealed class CreateApplicationValidator : AbstractValidator<CreateApplicationCommand>
{
    public CreateApplicationValidator()
    {
        RuleFor(command => command.ResourceType).Equal("application").OverridePropertyName("res-type");
    }
}
