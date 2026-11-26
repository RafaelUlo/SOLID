using IPL.SOLID.API.Application.Contexts;
using IPL.SOLID.API.Application.DomainEvents;
using IPL.SOLID.API.Domain.Entities;
using IPL.SOLID.API.Infrastructure.SQLite.Outbox;
using Microsoft.EntityFrameworkCore;

namespace IPL.SOLID.API.Infrastructure.SQLite.Contexts;

public sealed class SolidWriteContext : DbContext, ISolidWriteContext
{

    private readonly IDomainEventPublisher? _eventPublisher;

    public SolidWriteContext(DbContextOptions<SolidWriteContext> options, IDomainEventPublisher? eventPublisher = null)
        : base(options)
    {
        _eventPublisher = eventPublisher;
    }

    public DbSet<IPL.SOLID.API.Domain.Entities.Application> Applications { get; set; }

    public DbSet<Channel> Channels { get; set; }

    public DbSet<DataRecord> DataRecords { get; set; }

    public DbSet<Subscription> Subscriptions { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public DbSet<OutboxDelivery> OutboxDeliveries { get; set; }

    public DbSet<OutboxDeliveryAttempt> OutboxDeliveryAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SolidWriteContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        SaveChangesAsync(true, cancellationToken);

    public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();

    public override int SaveChanges(bool acceptAllChangesOnSuccess) =>
        SaveChangesAsync(acceptAllChangesOnSuccess).GetAwaiter().GetResult();

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();

        var entities = ChangeTracker
            .Entries<BaseEntity>()
            .ToList();

        foreach (var entry in entities.Where(entry => entry.State == EntityState.Added))
        {
            entry.Entity.CreatedAt = DateTime.UtcNow;
        }

        foreach (var entry in entities.Where(entry => entry.State == EntityState.Deleted))
        {
            if (entry.Entity is DataRecord record)
            {
                record.MarkDeleted();
            }
        }

        var events = entities
            .SelectMany(entry => entry.Entity.DomainEvents)
            .ToList();

        if (events.Count == 0)
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        var publisher = _eventPublisher
            ?? throw new InvalidOperationException("Domain event publisher is not configured.");

        await using var transaction = Database.CurrentTransaction is null
            ? await Database.BeginTransactionAsync(cancellationToken)
            : null;

        try
        {
            foreach (var domainEvent in events)
            {
                await publisher.PublishAsync(domainEvent, cancellationToken);
            }

            var count = await base.SaveChangesAsync(false, cancellationToken);

            if (transaction is not null)
            {
                await transaction.CommitAsync(cancellationToken);
            }
            if (acceptAllChangesOnSuccess)
            {
                ChangeTracker.AcceptAllChanges();
            }

            foreach (var entry in entities)
            {
                entry.Entity.ClearDomainEvents();
            }
            return count;
        }
        catch
        {
            if (transaction is not null)
            {
                await transaction.RollbackAsync(CancellationToken.None);
            }

            // Keep resource changes/events available for a retry; rebuild the outbox on that retry.
            var outboxEntriesToBeDetached = ChangeTracker.Entries()
                .Where(entry =>
                    entry.State == EntityState.Added &&
                    entry.Entity is OutboxDelivery or OutboxMessage)
                .ToList();

            foreach (var entry in outboxEntriesToBeDetached)
            {
                entry.State = EntityState.Detached;
            }

            throw;
        }
    }
}
