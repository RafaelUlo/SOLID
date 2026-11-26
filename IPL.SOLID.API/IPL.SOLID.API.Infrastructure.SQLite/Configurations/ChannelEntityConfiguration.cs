using IPL.SOLID.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IPL.SOLID.API.Infrastructure.SQLite.Configurations;

public sealed class ChannelEntityConfiguration : BaseEntityConfiguration<Channel>
{
    public override void Configure(EntityTypeBuilder<Channel> builder)
    {
        base.Configure(builder);
        builder.ToTable("Channel");
        builder.HasOne(c => c.Application).WithMany(a => a.Channels).HasForeignKey(c => c.ApplicationId);

        builder
            .HasMany(c => c.DataRecords)
            .WithOne(ci => ci.Channel)
            .HasForeignKey(ci => ci.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(c => c.Subscriptions)
            .WithOne(ci => ci.Channel)
            .HasForeignKey(ci => ci.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
