using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.Dtos.Channels;
using IPL.SOLID.API.Application.Mappings;
using IPL.SOLID.API.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Commands.Channels.CreateChannel;

public sealed class CreateChannelCommandHandler(ISolidWriteContext context)
    : ISemanticRequestHandler<CreateChannelCommand, ChannelDto>
{
    public async Task<SemanticResponse<ChannelDto>> Handle(
        CreateChannelCommand request, ResponseFactory<ChannelDto> response, CancellationToken token)
    {
        var parent = await context.Applications.SingleOrDefaultAsync(a => a.ResourceName == request.Application, token);
        if (parent is null) return response.NotFound("Parent resource not found.");


        var entity = new Channel(Guid.NewGuid(), request.ResourceName, parent.Id, request.Description);

        context.Channels.Add(entity);

        await context.SaveChangesAsync(token);

        return response.Created(ApplicationMapping.CreateChannelDto(entity));
    }
}
