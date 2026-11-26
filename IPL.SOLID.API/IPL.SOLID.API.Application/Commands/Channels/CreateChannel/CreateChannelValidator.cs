using FluentValidation;

namespace IPL.SOLID.API.Application.Commands.Channels.CreateChannel;

public sealed class CreateChannelValidator : AbstractValidator<CreateChannelCommand>
{
    public CreateChannelValidator()
    {
        RuleFor(command => command.ResourceType).Equal("channel").OverridePropertyName("res-type");
    }
}
