using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.Dtos.DataRecords;
using IPL.SOLID.API.Application.ErrorMessages;
using IPL.SOLID.API.Application.Mappings;
using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Commands.DataRecords.CreateDataRecord;

public sealed class CreateDataRecordCommandHandler(ISolidWriteContext context)
    : ISemanticRequestHandler<CreateDataRecordCommand, DataRecordDto>
{
    public async Task<SemanticResponse<DataRecordDto>> Handle(
        CreateDataRecordCommand request, ResponseFactory<DataRecordDto> response, CancellationToken cancellationToken)
    {
        var application = await context.Applications
            .FirstOrDefaultAsync(a => a.ResourceName == request.ApplicationName, cancellationToken);

        if (application is null)
        {
            var errorMessage = string.Format(
                ApplicationErrorMessage.APPLICATION_NOT_FOUND_ERROR_MESSAGE,
                request.ApplicationName);

            return response.NotFound(errorMessage);
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

        var entity = DataRecord.Create(
            Guid.NewGuid(),
            request.ResourceName,
            request.ContentType,
            request.Content,
            request.Expiration);

        channel.AddDataRecords(entity);
        context.Set<DataRecord>().Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return response.Created(ApplicationMapping.CreateDateRecordDto(entity));
    }
}
