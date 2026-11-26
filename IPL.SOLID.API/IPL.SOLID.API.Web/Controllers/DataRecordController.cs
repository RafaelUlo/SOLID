using IPL.SOLID.API.Application.Commands.DataRecords.DeleteDataRecord;
using IPL.SOLID.API.Application.Commands.DataRecords.CreateDataRecord;
using IPL.SOLID.API.Application.Dtos.DataRecords;
using IPL.SOLID.API.Application.Queries.DataRecords.GetDataRecords;
using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Web.Contexts;
using IPL.SOLID.API.Web.CustomAttributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IPL.SOLID.API.Web.Controllers;

[ApiController]
public sealed class DataRecordController(IMediator sender, DiscoveryContext discoveryContext) : BaseController(sender, discoveryContext)
{
    [HttpDelete("{application}/{channel}/{resource}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteDataRecord(string application, string channel, string resource, CancellationToken token)
    {
        var result = await Mediator.Send(new DeleteDataRecordCommand(application, channel, resource), token);
        return result.Success ? NoContent() : StatusCode(result.StatusCode, result);
    }

    [HttpGet("{application}/{channel}/{resource}")]
    public async Task<IActionResult> GetDataRecord(
        [FromRoute] string application,
        [FromRoute] string channel,
        [FromRoute] string resource,
        [FromHeader(Name = "solid-discovery")] string? discovery,
        CancellationToken token)
    {
        return await SendAsync(new GetDataRecordsQuery { ApplicationName = application, ChannelName = channel, ResourceName = resource }, token);
    }

    [ProducesResponseType(typeof(SemanticResponse<DataRecordDto>), StatusCodes.Status201Created)]
    [ResourceType("data-record")]
    [HttpPost("{application}/{channel}")]
    public async Task<IActionResult> CreateDataRecord(
    [FromRoute] string application,
    [FromRoute] string channel,
    [FromBody] CreateDataRecordCommand command,
    CancellationToken cancellationToken)
    {
        return await SendAsync(command with { ApplicationName = application, ChannelName = channel }, cancellationToken);
    }
}
