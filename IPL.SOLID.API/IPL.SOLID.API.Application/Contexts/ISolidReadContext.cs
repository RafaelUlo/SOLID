using IPL.SOLID.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ApplicationEntity = IPL.SOLID.API.Domain.Entities.Application;

namespace IPL.SOLID.API.Application.Contexts;

public interface ISolidReadContext
{
    DbSet<ApplicationEntity> Applications { get; }

    DbSet<Channel> Channels { get; }

    DbSet<DataRecord> DataRecords { get; }

    DbSet<Subscription> Subscriptions { get; }
}
