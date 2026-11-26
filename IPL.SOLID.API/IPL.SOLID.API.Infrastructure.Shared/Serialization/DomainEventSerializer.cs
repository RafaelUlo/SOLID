using IPL.SOLID.API.Domain.DomainEvents;
using IPL.SOLID.API.Domain.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace IPL.SOLID.API.Infrastructure.Shared.Serialization;

public static class DomainEventSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { ConfigureContract }
        }
    };

    public static string Serialize(IDomainEvent domainEvent) =>
        JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), Options);

    private static void ConfigureContract(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Type == typeof(BaseEntity))
        {
            typeInfo.PolymorphismOptions = new JsonPolymorphismOptions();
            typeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(DataRecord)));
        }

        if (typeof(IDomainEvent).IsAssignableFrom(typeInfo.Type))
        {
            foreach (var property in typeInfo.Properties)
                property.Name = property.Name switch
                {
                    nameof(IDomainEvent.Id) => "event-id",
                    nameof(IDomainEvent.OldState) => "old-state",
                    nameof(IDomainEvent.NewState) => "new-state",
                    nameof(IDomainEvent.CreatedAt) => "dt-event",
                    "Event" => "event",
                    "Evt" => "evt",
                    "Path" => "path",
                    _ => property.Name
                };
        }

        if (!typeof(BaseEntity).IsAssignableFrom(typeInfo.Type)) return;

        // Serialize resource data only; never traverse EF navigations or pending events.
        for (var index = typeInfo.Properties.Count - 1; index >= 0; index--)
        {
            var property = typeInfo.Properties[index];
            var jsonName = property.Name switch
            {
                nameof(BaseEntity.ResourceName) => "resource-name",
                nameof(BaseEntity.CreatedAt) => "dt-creation",
                nameof(DataRecord.ResourceType) => "res-type",
                nameof(DataRecord.ContentType) => "content-type",
                nameof(DataRecord.Content) => "content",
                nameof(DataRecord.Expiration) => "dt-expiration",
                _ => null
            };
            if (jsonName is null) typeInfo.Properties.RemoveAt(index);
            else property.Name = jsonName;
        }
    }
}
