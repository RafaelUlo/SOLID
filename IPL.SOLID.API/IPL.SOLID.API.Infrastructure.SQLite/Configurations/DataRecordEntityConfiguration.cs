using IPL.SOLID.API.Domain.Entities;
using IPL.SOLID.API.Domain.Enums;
using IPL.SOLID.API.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IPL.SOLID.API.Infrastructure.SQLite.Configurations;

public sealed class DataRecordEntityConfiguration : BaseEntityConfiguration<DataRecord>
{
    public override void Configure(EntityTypeBuilder<DataRecord> builder)
    {
        base.Configure(builder);
        builder.ToTable("DataRecord");

        var allowedContentTypes = string.Join(", ", ContentTypeSmartEnum.All.Select(v => $"'{v}'").Append("'text/plain'"));

        builder
            .ToTable(t =>
                t.HasCheckConstraint(
                    "CK_DataRecord_ContentType",
                    $"ContentType IN ({allowedContentTypes})"
                )
            );

        builder
            .Property(e => e.ContentType)
            .HasMaxLength(DomainEntitiesFieldsConstraints.MAX_CONTENT_TYPE_LENGTH);

        builder
            .HasOne(e => e.Channel)
            .WithMany(e => e.DataRecords)
            .HasForeignKey(e => e.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
