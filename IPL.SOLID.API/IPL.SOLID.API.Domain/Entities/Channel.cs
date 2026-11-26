using Krafted.Guards;

namespace IPL.SOLID.API.Domain.Entities;

public class Channel : BaseEntity
{
    public Guid? ApplicationId { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public Application? Application { get; private set; }

    public ICollection<DataRecord> DataRecords { get; private set; } = new HashSet<DataRecord>();

    public ICollection<Subscription> Subscriptions { get; private set; } = new HashSet<Subscription>();

    public Channel(Guid id, string resourceName, Guid applicationId, string description)
        : this(id, resourceName)
    {
        ApplicationId = applicationId;
        Description = description;
    }

    public Channel(Guid id, string resourceName)
        : base(id, resourceName)
    {
    }

    public void AddDataRecords(DataRecord dataRecord)
    {
        Guard.Against.Null(dataRecord);

        DataRecords.Add(dataRecord);
    }

    public void AddSubscription(Subscription subscription)
    {
        Guard.Against.Null(subscription);

        Subscriptions.Add(subscription);
    }
}
