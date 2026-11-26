using IPL.SOLID.API.Domain.Interfaces;

namespace IPL.SOLID.API.Domain.Entities;

public class Application : BaseEntity, IAggregateRoot
{
    public string Description { get; set; } = string.Empty;

    public ICollection<Channel> Channels { get; set; } = new HashSet<Channel>();

    public Application(Guid id, string resourceName)
        : base(id, resourceName)
    {
    }
}
