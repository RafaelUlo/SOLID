using IPL.SOLID.API.Domain.Entities;
using IPL.SOLID.API.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IPL.SOLID.API.Infrastructure.SQLite.Configurations;

public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Ignore(e => e.DomainEvents);

        builder.Property(e => e.ResourceName)
            .HasMaxLength(DomainEntitiesFieldsConstraints.MAX_RESOURCE_NAME_LENGTH);

        builder.HasIndex(e => e.ResourceName)
            .IsUnique();

        builder.Property(e => e.CreatedAt)
            .HasColumnOrder(100);
    }
}
