using IPL.SOLID.API.Application.Notifications;
using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.Dtos.Subscriptions;
using IPL.SOLID.API.Application.ErrorMessages;
using IPL.SOLID.API.Application.Mappings;
using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Domain.Entities;
using IPL.SOLID.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Commands.Subscriptions.CreateSubscription;

public sealed class CreateSubscriptionCommandHandler(ISolidWriteContext context)
    : ISemanticRequestHandler<CreateSubscriptionCommand, SubscriptionDto>
{
    public async Task<SemanticResponse<SubscriptionDto>> Handle(
        CreateSubscriptionCommand request, ResponseFactory<SubscriptionDto> response, CancellationToken cancellationToken)
    {
        if (!NotificationEndpoint.IsValid(request.Endpoint) || request.Evt is not (1 or 2))
            return response.BadRequest(new ResponseError("invalid_subscription",
                "Use evt 1 or 2 and an HTTP(S) callback URL or mqtt(s)://broker:port."));

        var application = await context.Applications
            .FirstOrDefaultAsync(e => e.ResourceName == request.ApplicationName, cancellationToken);

        if (application is null)
        {
            return response.NotFound(ApplicationErrorMessage.APPLICATION_NOT_FOUND_ERROR_MESSAGE);
        }

        var channel = await context.Channels.FirstOrDefaultAsync(c => c.ApplicationId == application.Id
            && c.ResourceName == request.ChannelName, cancellationToken);

        if (channel is null)
        {
            var errorMessage = string.Format(
                ApplicationErrorMessage.CHANNEL_NOT_FOUND_ERROR_MESSAGE,
                request.ChannelName,
                request.ApplicationName);

            return response.NotFound(errorMessage);
        }

        var entity = Subscription.Create(
            request.ResourceName,
            (EventTypeEnum)request.Evt,
            request.Endpoint,
            request.Expiration);

        channel.AddSubscription(entity);
        context.Set<Subscription>().Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return response.Created(ApplicationMapping.CreateSubscriptionDto(entity));
    }
}

