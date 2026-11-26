using IPL.SOLID.API.Application.Dtos.Applications;
using IPL.SOLID.API.Application.Dtos.Channels;
using IPL.SOLID.API.Application.Dtos.DataRecords;
using IPL.SOLID.API.Application.Dtos.Subscriptions;
using IPL.SOLID.API.Domain.Entities;

namespace IPL.SOLID.API.Application.Mappings;

public static class ApplicationMapping
{
    public static ApplicationDto CreateApplicationDto(Domain.Entities.Application application)
    {
        return new ApplicationDto()
        {
            ResourceName = application.ResourceName,
            Description = application.Description,
            CreatedAt = application.CreatedAt
        };
    }

    public static ChannelDto CreateChannelDto(Channel channel)
    {
        return new ChannelDto()
        {
            ResourceName = channel.ResourceName,
            Description = channel.Description,
            CreatedAt = channel.CreatedAt
        };
    }

    public static DataRecordDto CreateDateRecordDto(DataRecord dataRecord)
    {
        return new DataRecordDto()
        {
            ResourceName = dataRecord.ResourceName,
            ContentType = dataRecord.ContentType,
            Content = dataRecord.Content,
            Expiration = dataRecord.Expiration,
            CreatedAt = dataRecord.CreatedAt
        };
    }
    public static SubscriptionDto CreateSubscriptionDto(Subscription subscription)
    {
        return new SubscriptionDto()
        {
            ResourceName = subscription.ResourceName,
            Evt = (int)subscription.Evt,
            Endpoint = subscription.Endpoint,
            Expiration = subscription.Expiration,
            CreatedAt = subscription.CreatedAt
        };
    }
}
