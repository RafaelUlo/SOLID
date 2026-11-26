using IPL.SOLID.API.Domain.Enums;
using IPL.SOLID.API.Domain.Exceptions;

namespace IPL.SOLID.API.Domain.Entities;

public class Subscription : BaseEntity
{
    public DateTimeOffset? Expiration { get; set; }

    public Guid ChannelId { get; private set; }

    public EventTypeEnum Evt { get; private set; }

    public string Endpoint { get; private set; } = string.Empty;

    public Channel? Channel { get; private set; }

    internal Subscription(Guid id, string resourceName)
        : base(id, resourceName)
    {
    }

    internal Subscription(Guid id, string resourceName, EventTypeEnum evt, string endpoint, DateTimeOffset? expiration = null)
        : base(id, resourceName)
    {
        if (string.IsNullOrWhiteSpace(resourceName))
        {
            throw new DomainException("Resource name cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(endpoint))
        {
            throw new DomainException("Endpoint cannot be empty");
        }

        if (evt is not (EventTypeEnum.Creation or EventTypeEnum.Deletion))
            throw new DomainException("evt must be 1 (creation) or 2 (deletion).");

        Evt = evt;
        Endpoint = endpoint;
        Expiration = expiration;
    }

    protected Subscription()
    {
    }

    public static Subscription Create(
        string resourceName,
        EventTypeEnum evt,
        string endpoint,
        DateTimeOffset? expirtation = null)
    {
        return new Subscription(Guid.NewGuid(), resourceName, evt, endpoint, expirtation);
    }
}
