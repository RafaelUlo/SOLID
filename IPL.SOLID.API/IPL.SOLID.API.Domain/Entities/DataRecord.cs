using IPL.SOLID.API.Domain.DomainEvents.DateRecords;
using Krafted.Guards;

namespace IPL.SOLID.API.Domain.Entities;

public class DataRecord : BaseEntity
{
    public string ResourceType => "data-record";
    public DateTimeOffset? Expiration { get; set; }
    public Guid ChannelId { get; private set; }
    public string ContentType { get; protected set; }
    public string Content { get; private set; }
    public Channel? Channel { get; private set; }

    public static DataRecord Create(
        Guid id,
        string resourceName,
        string contentType,
        string content,
        DateTimeOffset? expiration = null)
    {
        var record = new DataRecord(id, resourceName, contentType, content, expiration);
        record.AddDomainEvent(new DataRecordCreatedEvent(null, record));
        return record;
    }

    internal DataRecord(
        Guid id,
        string resourceName,
        string contentType,
        string content,
        DateTimeOffset? expiration = null)
        : base(id, resourceName)
    {
        ContentType = contentType;
        Content = content;
        Expiration = expiration;
    }

    protected DataRecord()
    {
        ContentType = string.Empty;
        Content = string.Empty;
    }

    public void MarkDeleted()
    {
        if (!DomainEvents.OfType<DataRecordDeletedEvent>().Any())
            AddDomainEvent(new DataRecordDeletedEvent(this));
    }

    internal DataRecord Snapshot() => new(Id, ResourceName, ContentType, Content, Expiration)
    {
        ChannelId = ChannelId,
        CreatedAt = CreatedAt
    };

    public void DefineContentType(string contentType)
    {
        Guard.Against.Null(contentType);

        ContentType = contentType;
    }

    public void DefineContent(string content)
    {
        Guard.Against.EmptyOrWhiteSpace(content);

        Content = content;
    }
}
