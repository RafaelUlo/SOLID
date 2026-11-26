using IPL.SOLID.API.Application.Notifications;
using FluentValidation;

namespace IPL.SOLID.API.Application.Commands.Subscriptions.CreateSubscription;

public sealed class CreateSubscriptionValidator : AbstractValidator<CreateSubscriptionCommand>
{
    public CreateSubscriptionValidator()
    {
        RuleFor(command => command.ResourceType).Equal("subscription").OverridePropertyName("res-type");
        RuleFor(request => request.Evt).Must(evt => evt is 1 or 2)
            .WithMessage("evt must be 1 or 2.").OverridePropertyName("evt");
        RuleFor(request => request.Endpoint).Must(NotificationEndpoint.IsValid)
            .WithMessage("Use an HTTP(S) callback URL or mqtt(s)://broker:port.").OverridePropertyName("endpoint");
    }
}
