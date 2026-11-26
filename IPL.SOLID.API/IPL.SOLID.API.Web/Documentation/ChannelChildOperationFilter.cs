namespace IPL.SOLID.API.Web.Documentation;

using IPL.SOLID.API.Application.Commands.DataRecords.CreateDataRecord;
using IPL.SOLID.API.Application.Commands.Subscriptions.CreateSubscription;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using IPL.SOLID.API.Application.Responses;
using IPL.SOLID.API.Application.Dtos.DataRecords;
using IPL.SOLID.API.Application.Dtos.Subscriptions;
using Swashbuckle.AspNetCore.SwaggerGen;

public sealed class ChannelResourceOperationFilter
    : IOperationFilter
{
    public void Apply(
        OpenApiOperation operation,
        OperationFilterContext context)
    {
        var path = context.ApiDescription
            .RelativePath?
            .TrimEnd('/');

        if (!string.Equals(
                context.ApiDescription.HttpMethod,
                "POST",
                StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(
                path,
                "api/solid/{application}/{channel}",
                StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var dataRecordSchema =
            context.SchemaGenerator.GenerateSchema(
                typeof(CreateDataRecordCommand),
                context.SchemaRepository);

        var subscriptionSchema =
            context.SchemaGenerator.GenerateSchema(
                typeof(CreateSubscriptionCommand),
                context.SchemaRepository);

        operation.Summary = "Create a channel resource";

        operation.RequestBody = new OpenApiRequestBody
        {
            Required = true,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new()
                {
                    Schema = new OpenApiSchema
                    {
                        OneOf =
                        [
                            dataRecordSchema,
                            subscriptionSchema
                        ]
                    },
                    Examples = new Dictionary<string, OpenApiExample>
                    {
                        ["data-record"] = new()
                        {
                            Summary = "Create a humidity measurement",
                            Value = new OpenApiObject
                            {
                                ["res-type"] = new OpenApiString("data-record"),
                                ["resource-name"] = new OpenApiString("dr1"),
                                ["content-type"] = new OpenApiString("text/plain"),
                                ["content"] = new OpenApiString("32"),
                                ["dt-expiration"] = new OpenApiNull()
                            }
                        },
                        ["subscription"] = new()
                        {
                            Summary = "Subscribe to creation events",
                            Value = new OpenApiObject
                            {
                                ["res-type"] = new OpenApiString("subscription"),
                                ["resource-name"] = new OpenApiString("sub1"),
                                ["evt"] = new OpenApiInteger(1),
                                ["endpoint"] = new OpenApiString("http://localhost:9000/notifications"),
                                ["dt-expiration"] = new OpenApiNull()
                            }
                        }
                    }
                }
            }
        };

        operation.Description = "Use res-type data-record or subscription to select the resource to create.";
        operation.Responses["201"] = new OpenApiResponse
        {
            Description = "Created",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new()
                {
                    Schema = new OpenApiSchema
                    {
                        OneOf =
                        [
                            context.SchemaGenerator.GenerateSchema(typeof(SemanticResponse<DataRecordDto>), context.SchemaRepository),
                            context.SchemaGenerator.GenerateSchema(typeof(SemanticResponse<SubscriptionDto>), context.SchemaRepository)
                        ]
                    }
                }
            }
        };
    }
}