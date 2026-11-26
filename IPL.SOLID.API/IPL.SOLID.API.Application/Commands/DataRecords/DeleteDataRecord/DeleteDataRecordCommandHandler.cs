using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Application.Commands.DataRecords.DeleteDataRecord;

public sealed class DeleteDataRecordCommandHandler(ISolidWriteContext context)
    : ISemanticRequestHandler<DeleteDataRecordCommand, object>
{
    public async Task<SemanticResponse<object>> Handle(
        DeleteDataRecordCommand request, ResponseFactory<object> response, CancellationToken token)
    {
        var record = await context.Set<DataRecord>()
            .SingleOrDefaultAsync(record => record.ResourceName == request.ResourceName &&
                record.Channel!.ResourceName == request.ChannelName &&
                record.Channel.Application!.ResourceName == request.ApplicationName, token);
        if (record is null) return response.NotFound("Data record not found.");

        record.MarkDeleted();
        context.Set<DataRecord>().Remove(record);
        await context.SaveChangesAsync(token);
        return response.NoContent();
    }
}
