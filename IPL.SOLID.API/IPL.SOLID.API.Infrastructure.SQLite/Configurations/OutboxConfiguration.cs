using IPL.SOLID.API.Infrastructure.SQLite.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IPL.SOLID.API.Infrastructure.SQLite.Configurations;

public sealed class OutboxConfiguration :
    IEntityTypeConfiguration<OutboxMessage>,
    IEntityTypeConfiguration<OutboxDelivery>,
    IEntityTypeConfiguration<OutboxDeliveryAttempt>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");
        builder.HasKey(message => message.Id);
        builder.HasMany(message => message.Deliveries).WithOne(delivery => delivery.Message)
            .HasForeignKey(delivery => delivery.MessageId).OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<OutboxDelivery> builder)
    {
        builder.ToTable("OutboxDeliveries");
        builder.HasKey(delivery => delivery.Id);
        builder.HasIndex(delivery => new { delivery.MessageId, delivery.SubscriptionId }).IsUnique();
        builder.HasIndex(delivery => new { delivery.DeliveredAt, delivery.NextAttemptAt, delivery.LeaseUntil });
    }

    public void Configure(EntityTypeBuilder<OutboxDeliveryAttempt> builder)
    {
        builder.ToTable("OutboxDeliveryAttempts");
        builder.HasKey(attempt => attempt.Id);
        builder.HasOne<OutboxDelivery>().WithMany().HasForeignKey(attempt => attempt.DeliveryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
