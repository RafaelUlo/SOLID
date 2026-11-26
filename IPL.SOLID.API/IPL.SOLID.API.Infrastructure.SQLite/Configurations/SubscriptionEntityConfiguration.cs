using IPL.SOLID.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IPL.SOLID.API.Infrastructure.SQLite.Configurations;

public sealed class SubscriptionEntityConfiguration : BaseEntityConfiguration<Subscription>
{
    public override void Configure(EntityTypeBuilder<Subscription> builder)
    {
        base.Configure(builder);
        builder.ToTable("Subscription");
    }
}
